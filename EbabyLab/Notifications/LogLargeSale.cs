using eBabyServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    class LogLargeSale : Notification

    {
        public AuctionLogger AuctionLogger { get; set; }

        public LogLargeSale(AuctionLogger auctionLogger) { this.AuctionLogger = auctionLogger; }

        public override bool SendNotifications(Auction auction)
        {
            if (auction.CurrentPrice > 10000M)
            {
                AuctionLogger.Log(auction.Seller.UserName + "," + auction.HighestBidder.UserName + "," + auction.ItemDesc + "," +
                            auction.CurrentPrice.ToString() + "," + auction.BuyerAmount.ToString() + "," + auction.SellerAmount.ToString());
            }
            return true;
        }
    }
}
