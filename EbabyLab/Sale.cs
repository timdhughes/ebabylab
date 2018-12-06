using System;
using System.Collections.Generic;
using System.Text;
using EbabyLab;
using eBabyServices;

namespace eBabyLab
{
   public abstract class Sale
    {
        public abstract bool SendNotifications(Auction auction, PostOffice postOffice);
        public abstract bool UpdatePrice(Auction auction);
    }
}
