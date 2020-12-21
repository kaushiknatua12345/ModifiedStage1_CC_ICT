using System;

namespace ProfitShareApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RevenueShareCalculator shareCalculator = new RevenueShareCalculator();

            Console.WriteLine("Enter the number of persons who are partners in the company:");

            var partnersCount = byte.Parse(Console.ReadLine());
            bool isValidPartnersCount = shareCalculator.ValidateMembersCount(partnersCount);

            if (!isValidPartnersCount)
            {
                Console.WriteLine("Invalid Number of Partners Entered..Restart Your Application");
                Environment.Exit(0);
            }

            Console.WriteLine("Enter Profit Amount:");
            long totalProfit = long.Parse(Console.ReadLine());

            bool isValidProfitAmt = shareCalculator.ValidateProfitAmount(totalProfit);

            if (!isValidProfitAmt)
            {
                Console.WriteLine("Invalid Profit Amount..Restart Your Application");
                Environment.Exit(0);
            }

            Console.WriteLine("Enter total share valuation percentage of the company in the stock market:");

            int shareValuation = int.Parse(Console.ReadLine());

            bool isValidShareValuation = shareCalculator.ValidateShareValuation(shareValuation);

            if (!isValidShareValuation)
            {
                Console.WriteLine("Invalid Share Valuation in Stock Market Entered..Restart Your Application");
                Environment.Exit(0);
            }

            Console.WriteLine("Enter Share Ratio of Partners: ");
            string[] shareData = Console.ReadLine().Split(' ');

            int[] shareRatios = Array.ConvertAll(shareData, Convert.ToInt32);

            bool isValidShareRatios = shareCalculator.ValidateRevenueShareRatioValues(shareRatios);

            if (!isValidShareRatios)
            {
                Console.WriteLine("Invalid Share Ratio..Restart Your Application");
                Environment.Exit(0);
            }


            bool isValidShareValues = shareCalculator.ValidateRevenueShareRatios(shareValuation,shareRatios);

            if (!isValidShareValues)
            {
                Console.WriteLine("Negative Share Value Entered..Restart Your Application");
                Environment.Exit(0);
            }

            bool isValidShareNum = shareCalculator.ValidateNumberOfShares(partnersCount, shareRatios.Length);

            if (!isValidShareNum)
            {
                Console.WriteLine("Combined Share Ratio of all Partners does not match with Company's Share Valution in Stock Market");
                Environment.Exit(0);
            }

            Console.WriteLine("\nDisplay Profits made by the Partners:");

            double[] profitList = shareCalculator.CalculateProfitShareAmount(totalProfit, shareValuation, partnersCount, shareRatios);
            foreach (var profitData in profitList)
            {
                Console.WriteLine(profitData.ToString("0.00"));
            }

            double lowestProfit = shareCalculator.FindLowestProfitAmount(profitList);

            Console.WriteLine("\nThe Lowest Profit Value is:" + lowestProfit.ToString("0.00"));
        }
    }
}
