using Microsoft.Extensions.Configuration;
using SecretSanta.Infra.Mail.SPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Console.Config
{
    internal class MailServiceConfiguration : IMailServiceConfiguration
    {
        private const string ConfigSection = "SmtpConfig";

        public string SmtpClientUsername { get; }
        public string SmtpClientPassword { get; }
        public string SmtpServerHost { get; }
        public int SmtpServerPort { get; }

        public MailServiceConfiguration(IConfiguration configuration) 
        {
            SmtpClientUsername = configuration[$"{ConfigSection}:SmtpClientUsername"] ?? "";
            SmtpClientPassword = configuration[$"{ConfigSection}:SmtpClientPassword"] ?? "";
            SmtpServerHost = configuration[$"{ConfigSection}:SmtpServerHost"] ?? "";
            SmtpServerPort = int.Parse(configuration[$"{ConfigSection}:SmtpServerPort"] ?? "0");
        }
    }
}
