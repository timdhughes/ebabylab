using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    public class BidderPercFee : Fee
    {
        public BidderPercFee(decimal amount)
        {
            Amount = 1M + amount;
        }
        public override void Update(Auction auction)
        {
            auction.BuyerAmount *= Amount;
        }
    }
}
