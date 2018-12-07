using eBabyServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    public class LogOffHours : Notification
    {
        public AuctionLogger AuctionLogger { get; set; }

        public Hours OffHours { get; set; }

        public LogOffHours(AuctionLogger auctionLogger, Hours offHours) 
        {
            AuctionLogger = auctionLogger;
            OffHours = offHours;
        }

        public override bool SendNotifications(Auction auction)
        {
            if (OffHours == null)
                return false;

            if (OffHours.IsOffHours())
            {
                AuctionLogger.Log(auction.Seller.UserName + "," + auction.HighestBidder.UserName + "," + auction.ItemDesc + "," +
                            auction.CurrentPrice.ToString() + "," + auction.BuyerAmount.ToString() + "," + auction.SellerAmount.ToString());
            }
            return true;
        }

    }
}
