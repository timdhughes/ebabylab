using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    class BidderFixedFee : Fee
    {
        public BidderFixedFee(decimal amount)
        {
            Amount = amount;
        }
        public override void Update(Auction auction)
        {
            auction.BuyerAmount += Amount;
        }
    }
}
