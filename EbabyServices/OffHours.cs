using System;
namespace eBabyServices
{
    public class OffHours : Hours
    {
        private readonly Random m_Random = new Random();

        private OffHours()
        {
        }

        #region Hours Members

        public bool IsOffHours()
        {
            return (m_Random.Next() * 10.0 >= 5.0) ? true : false;
        }

        #endregion

        public static OffHours GetInstance()
        {
            return new OffHours();
        }
    }
}