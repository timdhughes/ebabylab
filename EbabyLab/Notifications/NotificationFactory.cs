using eBabyLab;
using eBabyServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{

    public static class NotificationFactory
    {

        public static PostOffice PostOffice { get; } = PostOffice.GetInstance();

        public static AuctionLogger CarSaleLogger { get; } = new AuctionLogger("car_sales.csv");

        public static AuctionLogger LargeSaleLogger { get; } = new AuctionLogger("large_sales.csv");

        public static AuctionLogger OffHoursLogger { get; } = new AuctionLogger("off_hours.csv");


        /// <summary>
        /// Decides which class to instantiate.
        /// </summary>
        public static List<Notification> GetInstance(Auction auction)
        {
            List<Notification> notifications = new List<Notification>();

            if (auction.HighestBidder == null)
                notifications.Add(new NoSale(PostOffice));
            else
                notifications.Add(new SuccessfulSale(PostOffice));

            notifications.Add(new LogCarSale(CarSaleLogger));
            notifications.Add(new LogLargeSale(LargeSaleLogger));
            notifications.Add(new LogOffHours(OffHoursLogger, auction.OffHours));

            return notifications;
        }
    }
}
