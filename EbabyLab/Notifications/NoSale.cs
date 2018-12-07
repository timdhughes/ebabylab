using System;
using System.Collections.Generic;
using System.Text;
using eBabyLab;
using eBabyServices;

namespace eBabyLab
{
    class NoSale : Notification
    {
        public PostOffice PostOffice { get; set; }

        public NoSale (PostOffice postOffice) { PostOffice = postOffice; }

        public override bool SendNotifications(Auction auction)
        {
            PostOffice.SendEMail(auction.Seller.UserEmail, "Sorry, your auction for " + auction.ItemDesc + " did not have any bidders.");
            return true;
        }
    }
}
