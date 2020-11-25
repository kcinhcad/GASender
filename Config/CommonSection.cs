using System.Configuration;

namespace Wallet.GASender.Config
{
    public class CommonSection : ConfigurationSection
    {
        [ConfigurationProperty("trackingId", IsRequired = true)]
        public string TrackingId
        {
            get { return (string)this["trackingId"]; }
            set { this["trackingId"] = value; }
        }

        [ConfigurationProperty("googleAnaliticsUrl", IsRequired = true)]
        public string GoogleAnaliticsUrl
        {
            get { return (string)this["googleAnaliticsUrl"]; }
            set { this["googleAnaliticsUrl"] = value; }
        }

        [ConfigurationProperty("googleAnaliticsTimeout", IsRequired = true)]
        public int GoogleAnaliticsTimeout
        {
            get { return (int)this["googleAnaliticsTimeout"]; }
            set { this["googleAnaliticsTimeout"] = value; }
        }
    }
}
