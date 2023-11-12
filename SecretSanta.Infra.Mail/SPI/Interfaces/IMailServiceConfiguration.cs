using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Mail.SPI.Interfaces
{
    public interface IMailServiceConfiguration
    {
        string SmtpClientUsername { get; }
        string SmtpClientPassword { get; }
        string SmtpServerHost { get; }
        int SmtpServerPort { get; }
    }
}
