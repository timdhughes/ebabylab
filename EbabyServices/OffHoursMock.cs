using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBabyServices
{
    public class OffHoursMock : Hours
    {
        public bool IsOffHours()
        {
            return true;
        }
    }
}
