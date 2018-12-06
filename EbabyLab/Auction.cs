using System;
using System.Collections.Generic;
using System.Text;

namespace EbabyLab
{

    public class Auction
    {
        public string Seller { get; private set; }
        public string ItemDesc { get; private set; }
        public double StartingPrice { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool Started { get; set; }
        public double CurrentPrice { get; set; }
        public string HighestBidder { get; set; }

        public static Auction Create(User seller, string itemdesc, double startingprice, DateTime starttime, DateTime endtime)
        {

            // constraints - user must be a seller, seller logged in
            // start time > now, end time> start time

            if (!seller.IsSeller || !seller.LoggedIn || starttime < DateTime.Now || endtime < starttime)
                return null;

            return new Auction
            {
                Seller = seller.UserName,
                ItemDesc = itemdesc,
                StartingPrice = startingprice,
                CurrentPrice = startingprice,
                StartTime = starttime,
                EndTime = endtime
            };
        }

        public void OnStart()
        {
            this.Started = true;
        }

        public enum BidStatus { Accepted, UserNotLoggedIn, AuctionNotActice, BidTooLow }

        public bool Bid(User user, double bidAmount, out BidStatus status)
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
            HighestBidder = user.UserName;
            return true;

        }
    }
}
