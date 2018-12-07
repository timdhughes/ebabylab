using eBabyLab;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{

    public static class SaleFactory
    {
        /// <summary>
        /// Decides which class to instantiate.
        /// </summary>
        public static Sale GetInstance(User highestBidder)
        {
            if (highestBidder == null)
                return new NoSale();
            else
                return new SuccessfulSale();

        }
    }
}
