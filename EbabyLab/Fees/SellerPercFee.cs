using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    class SellerPercFee : Fee
    {
        public SellerPercFee(decimal amount)
        {
            Amount = 1M - amount;
        }
        public override void Update(Auction auction)
        {
            auction.SellerAmount *= Amount;
        }
    }
}
