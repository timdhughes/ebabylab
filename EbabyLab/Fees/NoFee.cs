using eBabyLab;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    public class NoFee : Fee
    {
        public NoFee()
        {

        }

        public override void Update(Auction auction)
        {
            auction.BuyerAmount = auction.CurrentPrice;
            auction.SellerAmount = auction.CurrentPrice;
        }
    }
}
