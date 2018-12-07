using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBabyServices
{
    public static class HoursFactory
    {

        public static Hours GetHours(bool testMode)
        {
            if (testMode)
                return new OffHoursMock();
            else
                return OffHours.GetInstance();
        }
    }
}
