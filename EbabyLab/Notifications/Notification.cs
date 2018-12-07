using System;
using System.Collections.Generic;
using System.Text;
using eBabyLab;
using eBabyServices;

namespace eBabyLab
{
   public abstract class Notification
    {
        public abstract bool SendNotifications(Auction auction);
        //public abstract bool UpdatePrice(Auction auction);
    }
}
