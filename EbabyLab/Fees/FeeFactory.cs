using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    class FeeFactory
    {
        public static List<Fee> GetFees(Auction auction)
        {
            List<Fee> fees = new List<Fee>();

            if (auction.HighestBidder == null)
                return fees;

            fees.Add(new NoFee());
            fees.Add(new SellerPercFee(0.02M));
            
            if (auction.Category == Auction.ItemCategory.Car)
            {                
                if (auction.CurrentPrice > 50000.0M)
                    fees.Add(new BidderPercFee(0.04M));

                fees.Add(new BidderFixedFee(1000.0M));
            }
            else if (auction.Category == Auction.ItemCategory.DownloadableSoftware)
            {
                // no shipping fee
            }
            else
            {
                // default shipping fee
                fees.Add(new BidderFixedFee(10.00M));
            }

            return fees;
        }
    }
}
