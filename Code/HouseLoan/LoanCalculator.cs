using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseLoan
{
    public class LoanCalculator
    {
        /// <summary>
        /// 等额本息
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="monthlyInterest"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public static double Calculate1(double amount, double monthlyInterest, int months)
        {
            double temp = Math.Pow(1 + monthlyInterest, months);
            return (amount * monthlyInterest * temp) / (temp - 1);
        }

        /// <summary>
        /// 等额本金
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="monthlyInterest"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public static double[] Calculate2(double amount, double monthlyInterest, int months)
        {
            double paiedBase = 0;
            double[] pay = new double[months];
            for (int i = 0; i < months; i++)
            {
                pay[i] = (amount / (double)months) + (amount - paiedBase) * monthlyInterest;
                paiedBase += amount / (double)months;
            }
            return pay;
        }
    }
}
