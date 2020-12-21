using NUnit.Framework;
using System.Reflection;
using System.Linq;

namespace ProfitShareApp.Test
{
    [TestFixture]
    public class RevenueShareCalculatorTests
    {

        private MethodInfo validateMembersCount;
        private MethodInfo validateProfitAmount;
        private MethodInfo validateShareValuation;
        private MethodInfo validateNumberOfShares;
        private MethodInfo validateRevenueShareRatioValues;
        private MethodInfo validateRevenueShareRatios;
        private MethodInfo calculateProfitShareAmount;
        private MethodInfo findLowestProfitAmount;

        private object revenueShareCalculatorObject;

        [SetUp]
        public void SetUp()
        {
            Assembly assembly = Assembly.Load("ProfitShareApp");
            revenueShareCalculatorObject = assembly.CreateInstance
                (assembly.GetTypes().Where(type => type.Name == "RevenueShareCalculator").FirstOrDefault()?.FullName,
                false, BindingFlags.CreateInstance, null, null, null, null);

            validateMembersCount = revenueShareCalculatorObject.GetType().GetMethod("ValidateMembersCount");
            validateProfitAmount = revenueShareCalculatorObject.GetType().GetMethod("ValidateProfitAmount");
            validateShareValuation = revenueShareCalculatorObject.GetType().GetMethod("ValidateShareValuation");
            validateNumberOfShares = revenueShareCalculatorObject.GetType().GetMethod("ValidateNumberOfShares");
            validateRevenueShareRatioValues = revenueShareCalculatorObject.GetType().GetMethod("ValidateRevenueShareRatioValues");
            validateRevenueShareRatios = revenueShareCalculatorObject.GetType().GetMethod("ValidateRevenueShareRatios");
            calculateProfitShareAmount = revenueShareCalculatorObject.GetType().GetMethod("CalculateProfitShareAmount");
            findLowestProfitAmount = revenueShareCalculatorObject.GetType().GetMethod("FindLowestProfitAmount");
        }

        [Test]
        public void ValidateMembersCount_ValidMembersCount_ReturnTrue()
        {
            int partnerCount = 3;

            var result = validateMembersCount.Invoke(revenueShareCalculatorObject, new object[] { partnerCount });

            Assert.That(result, Is.True);
        }

        [Test]
        public void ValidateMembersCount_InvalidMembersCount_ReturnFalse()
        {
            int partnerCount = 10;

            var result = validateMembersCount.Invoke(revenueShareCalculatorObject, new object[] { partnerCount });

            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateProfitAmount_ValidProfitAmount_ReturnTrue()
        {
            long profitAmount = 10000;

            var result = validateProfitAmount.Invoke(revenueShareCalculatorObject, new object[] { profitAmount });

            Assert.That(result, Is.True);
        }

        [Test]
        public void ValidateProfitAmount_InvalidProfitAmount_ReturnFalse()
        {
            long profitAmount = 1;

            var result = validateProfitAmount.Invoke(revenueShareCalculatorObject, new object[] { profitAmount });

            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateShareValuation_ValidCompanyShareValuation_ReturnTrue()
        {
            int shareValuation = 5;

            var result = validateShareValuation.Invoke(revenueShareCalculatorObject, new object[] { shareValuation });

            Assert.That(result, Is.True);
        }

        [Test]
        public void ValidateShareValuation_InvalidCompanyShareValuation_ReturnFalse()
        {
            int shareValuation = 21;

            var result = validateShareValuation.Invoke(revenueShareCalculatorObject, new object[] { shareValuation });

            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateNumberOfShares_PartnersCountEqualsToSharesCount_ReturnTrue()
        {
            int partnersCount = 4;
            int sharesCount = 4;

            var result = validateNumberOfShares.Invoke(revenueShareCalculatorObject, new object[] { partnersCount, sharesCount });

            Assert.That(result, Is.True);
        }

        [Test]
        public void ValidateNumberOfShares_PartnersCountNotEqualsToSharesCount_ReturnFalse()
        {
            int partnersCount = 4;
            int sharesCount = 5;

            var result = validateNumberOfShares.Invoke(revenueShareCalculatorObject, new object[] { partnersCount, sharesCount });

            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateRevenueShareRatioValues_ShareRatioValuesGreaterThanZero_ReturnTrue()
        {

            int[] shareRatios = { 1, 2, 3 };

            var result = validateRevenueShareRatioValues.Invoke(revenueShareCalculatorObject, new object[] { shareRatios });

            Assert.That(result, Is.True);
        }

        [Test]
        public void ValidateRevenueShareRatioValues_ShareRatioValuesLessThanZero_ReturnFalse()
        {

            int[] shareRatios = { 1, 2, -1 };

            var result = validateRevenueShareRatioValues.Invoke(revenueShareCalculatorObject, new object[] { shareRatios });

            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateRevenueShareRatios_ShareRatiosSumIsEqualToShareValuation_ReturnTrue()
        {
            int[] shareRatios = { 1, 2, 2 };
            int shareValuation = 5;

            var result = validateRevenueShareRatios.Invoke(revenueShareCalculatorObject, new object[] { shareValuation, shareRatios });

            Assert.That(result, Is.True);
        }

        [Test]
        public void ValidateRevenueShareRatios_ShareRatiosSumIsNotEqualToShareValuation_ReturnFalse()
        {
            int[] shareRatios = { 1, 2, 3 };
            int shareValuation = 5;

            var result = validateRevenueShareRatios.Invoke(revenueShareCalculatorObject, new object[] { shareValuation, shareRatios });

            Assert.That(result, Is.False);
        }

        [Test]
        public void CalculateProfitShareAmount_OnValidInputs_ReturnsProfitShareValue()
        {
            long profitAmount = 10000;
            int shareValuation = 10;
            int partnersCount = 3;
            int[] shareRatios = { 3, 3, 4 };

            double[] expectedResult = { 3000.00, 3000.00, 4000.00 };

            var actualResult = calculateProfitShareAmount.Invoke(revenueShareCalculatorObject, new object[] { profitAmount, shareValuation, partnersCount, shareRatios });

            CollectionAssert.AreEquivalent(expectedResult, (double[])actualResult);
        }

        [Test]
        public void FindLowestProfitAmount_OnValidInput_ReturnsLowestProfitAmount()
        {
            double[] profitShares = { 3000.00, 1500.00, 4000.00 };
            double expectedResult = 1500.00;

            var actualResult = findLowestProfitAmount.Invoke(revenueShareCalculatorObject, new object[] { profitShares });

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
