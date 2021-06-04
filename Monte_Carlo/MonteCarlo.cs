using System;
using System.Collections.Generic;

namespace Monte_Carlo
{
    class Program
    {
        static int nStocks;                             // number of stocks
        static double errorMargin = 0.01;                // acceptable error margin
        static int simulations = 2000000;               // total monte carlo simulations to be run
        
        /// Checks whether a portfolio is valid or not
        /// 
        /// <param name="portfolio"></param>
        /// <param name="stockPrice"></param>
        /// <param name="portfolioValue"></param>
        /// <returns>bool Validity of current portfolio</returns>
        public static bool checkValidPortfolio(List <int> portfolio, List<int> stockPrice, int portfolioValue) {
            double lowestAcceptable, highestAcceptable;
            lowestAcceptable = (1-errorMargin)*portfolioValue;
            highestAcceptable = (1+errorMargin)*portfolioValue;
            int currentPortfolioValue = 0;
            for(int i=0; i<nStocks; i++) {
                currentPortfolioValue += portfolio[i]*stockPrice[i];
            }
            return (currentPortfolioValue > lowestAcceptable && currentPortfolioValue < highestAcceptable);
        }
        static void printList(List <int> l) {
            l.ForEach(Console.WriteLine);
        }
        static void Main(string[] args)
        {
            int portfolioValue;                         // value of portfolio, user input
            List<int> stockPrice = new List<int >();     // stock prices of each stock
            List<int> lower = new List<int>();          // lowest number of units for a particular stock
            List <int> upper = new List<int>();         // highest number of units for a particular stock
            portfolioValue = Convert.ToInt32(Console.ReadLine());
            nStocks = Convert.ToInt32(Console.ReadLine());
            for(int i=0; i<nStocks; i++) {
                int min = int.Parse(Console.ReadLine());
                int max = int.Parse(Console.ReadLine());
                int price = int.Parse(Console.ReadLine());
                stockPrice.Add(price);
                lower.Add((min*portfolioValue)/(100*price));
                upper.Add((max*portfolioValue)/(100*price));
            }
            Random rand = new Random();
            ListComparator lc = new ListComparator();
            HashSet <List <int> > generatedPortfolios = new HashSet< List <int> >(lc);
            for (int j = 0; j < simulations; j++)
            {
                List <int> validPortfolio = new List<int>() ;
                for (int i = 0; i < nStocks; i++)
                {
                    validPortfolio.Add(rand.Next(lower[i], upper[i]));
                }
                if (generatedPortfolios.Contains(validPortfolio) || checkValidPortfolio(validPortfolio, stockPrice, portfolioValue) == false) continue;
                else generatedPortfolios.Add(validPortfolio);
            }
            Console.WriteLine($"Valid portfolios generated: {generatedPortfolios.Count}");
            Console.WriteLine($"Efficiency: {100 * ((decimal)generatedPortfolios.Count/(decimal)simulations)}");

        }
    }

    /// <summary>
    /// Compares two lists and removes duplicates
    /// </summary>
    class ListComparator : EqualityComparer<List <int> >
    {
        public override bool Equals(List<int> x, List<int> y)
        {
            bool flag = true;
            for (int i = 0; i < x.Count; i++) {
                if(x[i] != y[i]) {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public override int GetHashCode(List <int> x)
        {
            int hashCode = 0;
            for (int i = 0; i < x.Count; i++)
            {
                hashCode += (x[i] * (int)Math.Pow(2, i));
            }
            return hashCode;
        }
    }

    

}
