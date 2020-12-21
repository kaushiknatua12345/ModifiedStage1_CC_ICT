using System.Linq;

namespace ProfitShareApp
{
    public class RevenueShareCalculator
    {
        public bool ValidateMembersCount(int partnersCount)
        {
            return partnersCount >= 2 && partnersCount <= 5;
        }

        public bool ValidateProfitAmount(long profitAmount)
        {
            return profitAmount >= 10000;
        }

        public bool ValidateShareValuation(int shareValuation)
        {
            return shareValuation >=5 && shareValuation <= 20;
        }

        public bool ValidateNumberOfShares(int partnersCount, int sharesCount)
        {
            return partnersCount == sharesCount;
        }

        public bool ValidateRevenueShareRatioValues(int[] shareRatios)
        {
            return shareRatios.All(x => x > 0);
        }

        public bool ValidateRevenueShareRatios(int shareValuation, int[] shareRatios)
        {
            return shareRatios.Sum() == shareValuation;
        }

        public double[] CalculateProfitShareAmount(long profitAmount, int shareValuation, int partnersCount, int[] shareRatios)
        {
            double[] profitShares = new double[partnersCount];

            for (int i = 0; i < partnersCount; i++)
            {
                profitShares[i] = (double)(profitAmount * shareRatios[i]) / shareValuation;
            }

            return profitShares;
        }

        public double FindLowestProfitAmount(double[] profitShares)
        {
            return profitShares.Min();
        }
    }
}
