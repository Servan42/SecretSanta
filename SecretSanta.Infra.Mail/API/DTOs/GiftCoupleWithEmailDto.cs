using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Mail.API.DTOs
{
    public class GiftCoupleWithEmailDto
    {
        public string Gifter { get; set; }
        public MailAddress GifterEmail { get; set; }
        public string Receiver { get; set; }
    }
}
