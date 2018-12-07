using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    public abstract class Fee
    {
        public virtual decimal Amount { get; set; }

        public abstract void Update(Auction auction);
    }
}
