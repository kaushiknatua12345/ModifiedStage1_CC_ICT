using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTDSCalculator.BLL
{
    public class TDSCalculation
    {
        public double CalculateTaxAmount(double empCTC)
        {
            double tdsAmount;

            if (empCTC <= 300000)
            {
                tdsAmount = 0;
            }
            else if (empCTC > 300000 && empCTC <= 1000000)
            {
                tdsAmount = (empCTC * 10) / 100;
            }
            else
            {
                tdsAmount = (empCTC * 15) / 100;
            }
            
            return tdsAmount;
        }
    }
}
