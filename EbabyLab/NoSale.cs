using System;
using System.Collections.Generic;
using System.Text;
using EbabyLab;
using eBabyServices;

namespace eBabyLab
{
    class NoSale : Sale
    {
        public override bool SendNotifications(Auction auction, PostOffice postOffice)
        {
            postOffice.SendEMail(auction.Seller.UserEmail, "Sorry, your auction for " + auction.ItemDesc + " did not have any bidders.");
            return true;
        }

        public override bool UpdatePrice(Auction auction)
        {
            return true;
        }
    }
}
