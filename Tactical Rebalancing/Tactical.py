import  yfinance as yf
import csv
import pandas as pd
import zipline

data = yf.download("AAPL WMT PFE TSLA", start="2010-01-01", end="2021-05-31")
data.to_csv('stock_data.csv')


# with open('protagonist.csv', 'w', newline='') as file:
#     writer = csv.writer(file)
#     writer.writerows(data)