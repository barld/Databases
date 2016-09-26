using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeDataModels.DataLogic
{
    public static class BSN
    {
        /// <summary>
        /// 11 check for BSN
        /// </summary>
        /// <param name="BSN"></param>
        /// <returns></returns>
        /// <remarks>https://nl.wikipedia.org/wiki/Burgerservicenummer</remarks>
        public static bool IsValidBSN(this int BSN)
        {
            return BSNSum(BSN, 1) % 11 == 0;
        }

        private static int BSNSum(int BSN, int position)
        {
            if(position == 1)
            {
                return (-1 * (BSN % 10)) + BSNSum(BSN / 10, position + 1);
            }
            else if(position < 10)
            {
                return (position * (BSN % 10)) + BSNSum(BSN / 10, position + 1);
            }
            else
            {
                return 0;
            }
        }
    }
}
