using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Mail.API.Model
{
    public class GiftCoupleWithEmail
    {
        public string Gifter { get; set; }
        public string GifterEmail { get; set; }
        public string Receiver { get; set; }

        public MailMessage GetMailObject(string from)
        {
            return new MailMessage(from, GifterEmail)
            {
                Subject = "Secret Santa",
                Body = $"Hello {Gifter}, for this year's secret santa, you must prepare a present for {Receiver}." +
                $"\n\nThis message was sent by a program. Source: https://github.com/Servan42/SecretSanta",
            };
        }
    }
}
