using eBabyServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    public class AuctionManager
    {
        public List<Auction> Auctions { get; private set; }

        public Hours OffHours { get; private set; }


        public AuctionLogger CarSaleLogger { get; set; }

        public AuctionManager(Hours offHours)
        {
            OffHours = offHours;
            NotificationFactory.CarSaleLogger.Log("Seller, HighestBidder, ItemDesc, CurrentPrice, BuyerAmount, SellerAmount");
            NotificationFactory.LargeSaleLogger.Log("Seller, HighestBidder, ItemDesc, CurrentPrice, BuyerAmount, SellerAmount");

        }

        public AuctionManager() : this(HoursFactory.GetHours(false))
        {
        }

        public void AddAuction(Auction auction)
        {
            Auctions.Add(auction);
            
        }
    }
}
