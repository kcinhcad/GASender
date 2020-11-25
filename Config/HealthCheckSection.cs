using System.Configuration;

namespace Wallet.GASender.Config
{
    /// <summary>
    /// Настройки api мониторинга работоспособности сервиса
    /// </summary>
    public class HealthCheckSection : ConfigurationSection
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }
    }
}
