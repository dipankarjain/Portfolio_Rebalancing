"""
    Title: Buy and Hold (NYSE)
    Description: This is a long only strategy which rebalances the 
        portfolio weights every month at month start.
    Style tags: Systematic
    Asset class: Equities, Futures, ETFs, Currencies and Commodities
    Dataset: NYSE Daily or NYSE Minute
"""
# Zipline
from zipline.api import(    record,
                            symbol,
                            order_target_percent,
                            order_value,
                            schedule_function,
                            date_rules,
                            time_rules
                       )

import talib as ta
import numpy as np
import math, datetime

def initialize(context):
    """
        A function to define things to do at the start of the strategy
    """
    
    # universe selection
    context.long_portfolio = [
                               symbol('AMZN'),
                               symbol('AAPL'),
                               symbol('WMT'),
                               symbol('MU'),
                               symbol('BAC'),
                               symbol('KO'),
                               symbol('BA'),
                               symbol('AXP')
                             ]
    context.initial_portfolio_set = False
    context.portfolio_count = len(context.long_portfolio)
    context.vol_limit = 30
    context.ppo_limit = 2.0
    context.ppo_daywise = [0.0]
    context.date = datetime.datetime(2008, 5, 30)
    context.portfolio_close = [100000.0]
    # Call rebalance function on the first trading day of each month after 2.5 hours from market open
    schedule_function(
        rebalance
    )

def return_stock_weights(context) :
    """
        Find the relative weights of the stocks in our portfolio
    """
    stock_values = []
    for security in context.long_portfolio :
        security_details = context.portfolio.positions[security]
        stock_values.append(security_details.cost_basis * security_details.amount)
    return stock_values/np.sum(stock_values)

def calculate_vol(context, data) :
    """
        Function to calculate volatility for a stock for a week, month or year in percentage terms
        
        Params :
        data : the data variable to access stock data
        security : the underlying security
        duration : the duration to calculate volality("W": week, "M": month, "Y": year)

        Returns :
        Volatility as a percentage of stock price
    """
    daily_returns = []
    for security in context.long_portfolio:
        px = data.history(security,  "close", 30, "1d")
        normalized_prices = px.values/px.values[0]
        stock_returns = [0.0]
        for i in xrange(1, len(normalized_prices)):
            stock_returns.append(normalized_prices[i] / normalized_prices[i-1] - 1)
        daily_returns.append(stock_returns)
    
    # create the correlation matrix
    corr_matrix = np.corrcoef(daily_returns)
 
    # portfolio weights
    w = np.array(return_stock_weights(context))
 
    # portfolio volatility
    return np.sqrt(w.T.dot(corr_matrix).dot(w))


def calculate_portfolio_average(context, measure) :
    """
        Calculate weighted parameters for the portfolio given the parameter
        
        Params :
        context : context variable
        measure : (numpy array) The measures whose average needs to be found
    """
    stock_values = return_stock_weights(context)
    if sum(stock_values) == 0:
        return 0
    return np.average(measure, weights=stock_values)

def rebalance(context,data):
    """
        A function to rebalance the portfolio, passed on to the call
        of schedule_function above.
    """
    
    individual_vol = []
    individual_ppo = []
    individual_macd = []
    
    # Set the initial portfolio equally among all long stocks 
    if context.initial_portfolio_set == False :
        for security in context.long_portfolio:
            order_target_percent(security, 0.7/len(context.long_portfolio))
        context.initial_portfolio_set = True

    for security in context.long_portfolio:
        px = data.history(security,  "close", 100, "1d")
        # Find the portfolio percentage osscilation of the portfolio
        ppo = ta.PPO(px.values, 5, 90)
        individual_ppo.append(ppo[ppo.size-1])

        macd, macdsig, macdhist = ta.MACD(px.values, 5, 90)
        individual_macd.append(macd[macd.size-1])

    portfolio_vol = calculate_vol(context, data)
    portfolio_ppo = calculate_portfolio_average(context, individual_ppo)
    portfolio_macd = calculate_portfolio_average(context, individual_macd)
    portfolio_rsi = ta.RSI(np.array(context.portfolio_close))
    if (portfolio_rsi[portfolio_rsi.size-1] < 30 ):
        print(context.date, portfolio_rsi[portfolio_rsi.size-1])
        for security in context.long_portfolio:
            order_target_percent(security, 0.7/len(context.long_portfolio))
    # increment date
    context.date += datetime.timedelta(days=1)
    context.portfolio_close.append(context.portfolio.portfolio_value)