using System;
using System.Collections.Generic;
using System.Text;
using eBabyLab;
using eBabyServices;
namespace eBabyLab
{

    public class Auction
    {
        public enum ItemCategory { Misc, Car, DownloadableSoftware };

        public decimal SellerAmount{get;  set; }

        public PostOffice PostOffice { get; private set; }


        public User Seller { get; private set; }
        public string ItemDesc { get; private set; }

        public ItemCategory Category { get; set; }

        public decimal StartingPrice { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool Started { get; set; }
        public decimal CurrentPrice { get; set; }
        public User HighestBidder { get; set; }
        public decimal BuyerAmount { get; set; }

        public Hours OffHours { get; set; }

        public static Auction Create(User seller, string itemdesc, decimal startingprice, DateTime starttime, DateTime endtime)
        {

            // constraints - user must be a seller, seller logged in
            // start time > now, end time> start time

            if (!seller.IsSeller || !seller.LoggedIn || starttime < DateTime.Now || endtime < starttime)
                return null;



            return new Auction
            {
                Seller = seller,
                ItemDesc = itemdesc,
                StartingPrice = startingprice,
                CurrentPrice = startingprice,
                StartTime = starttime,
                EndTime = endtime,
                PostOffice = PostOffice.GetInstance()
            };
        }

        public static Auction Create(User seller, string itemdesc, decimal startingprice, DateTime starttime, DateTime endtime, ItemCategory category)
        {
            Auction auction = Create(seller, itemdesc, startingprice, starttime, endtime);
            auction.Category = category;
            return auction;
        }

        public void OnStart()
        {
            Started = true;
        }

        public enum BidStatus { Accepted, UserNotLoggedIn, AuctionNotActice, BidTooLow }

        public bool Bid(User user, decimal bidAmount, out BidStatus status)
        {
            if (!user.LoggedIn)
            {
                status = BidStatus.UserNotLoggedIn;
                return false;
            }
            if (!Started)
            {
                status = BidStatus.AuctionNotActice;
                return false;
            }

            if (bidAmount <= CurrentPrice)
            {
                status = BidStatus.BidTooLow;
                return false;

            }
            status = BidStatus.Accepted;
            CurrentPrice = bidAmount;
            HighestBidder = user;
            return true;

        }

        public void OnClose()
        {
            var fees = FeeFactory.GetFees(this);
            foreach (var fee in fees)
                fee.Update(this);

            List<Notification> notifications = NotificationFactory.GetInstance(this);
            foreach (Notification notification in notifications)            
                notification.SendNotifications(this);

                        
        }
    }
}
