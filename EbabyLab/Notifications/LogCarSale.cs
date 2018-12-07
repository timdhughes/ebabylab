using eBabyServices;
using System;
using System.Collections.Generic;
using System.Text;


namespace eBabyLab
{
    class LogCarSale : Notification
    {
        public AuctionLogger AuctionLogger { get; set; }

        public LogCarSale(AuctionLogger auctionLogger) { this.AuctionLogger = auctionLogger; }
        public override bool SendNotifications(Auction auction)
        {
            if (auction.Category == Auction.ItemCategory.Car)
            {
                AuctionLogger.Log(auction.Seller.UserName + "," + auction.HighestBidder.UserName + "," + auction.ItemDesc + "," +
                            auction.CurrentPrice.ToString() + "," + auction.BuyerAmount.ToString() + "," + auction.SellerAmount.ToString());
            }
            return true;
        }
    }
}
