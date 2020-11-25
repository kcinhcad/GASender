using System;
using System.Configuration;

namespace Wallet.GASender.Config
{
    public class PaymentsSection : ConfigurationSection
    {
        [ConfigurationProperty("w1ConnectionString", IsRequired = true)]
        public string W1ConnectionString
        {
            get { return (string)this["w1ConnectionString"]; }
            set { this["w1ConnectionString"] = value; }
        }

        [ConfigurationProperty("dateFrom", IsRequired = false)]
        public DateTime DateFrom
        {
            get { return (DateTime)this["dateFrom"]; }
            set { this["dateFrom"] = value; }
        }

        [ConfigurationProperty("interval", IsRequired = true)]
        public int Interval
        {
            get { return (int)this["interval"]; }
            set { this["interval"] = value; }
        }

        [ConfigurationProperty("startTime", IsRequired = true)]
        public TimeSpan StartTime
        {
            get { return (TimeSpan)this["startTime"]; }
            set { this["startTime"] = value; }
        }
    }
}
