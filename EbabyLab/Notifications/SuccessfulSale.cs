using System;
using System.Collections.Generic;
using System.Text;
using eBabyLab;
using eBabyServices;

namespace eBabyLab
{
    class SuccessfulSale : Notification
    {
        public PostOffice PostOffice { get; set; }
        public SuccessfulSale(PostOffice postOffice) { PostOffice = postOffice; }

        public override bool SendNotifications(Auction auction)
        {
            PostOffice.SendEMail(auction.HighestBidder.UserEmail, "Congratulations! You won an auction for a " + auction.ItemDesc + " from " + auction.Seller.UserName + " for " + auction.CurrentPrice.ToString("C2") + ".");
            PostOffice.SendEMail(auction.Seller.UserEmail, "Your " + auction.ItemDesc + " auction sold to bidder " + auction.HighestBidder.UserEmail + " for " + auction.CurrentPrice.ToString("C2") + ".");
            return true;
        }

     /*   public override bool UpdatePrice(Auction auction)
        {
            auction.SellerAmount = .98M * auction.CurrentPrice;

            if (auction.Category == Auction.ItemCategory.Car)
            {
                auction.BuyerAmount = auction.CurrentPrice;

                if (auction.CurrentPrice > 50000.0M)
                {
                    auction.BuyerAmount *= 1.04M;
                }
                auction.BuyerAmount += 1000.0M;
            }

            else if (auction.Category == Auction.ItemCategory.DownloadableSoftware)
            {
                auction.BuyerAmount = auction.CurrentPrice;
            }
            else
            {
                auction.BuyerAmount = auction.CurrentPrice + 10.00M;
            }

            return true;
        }*/
    }
}
