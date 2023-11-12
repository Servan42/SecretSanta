using AutoMapper;
using SecretSanta.Infra.Mail.API.DTOs;
using SecretSanta.Infra.Mail.API.Interfaces;
using SecretSanta.Infra.Mail.API.MapperProfile;
using SecretSanta.Infra.Mail.API.Model;
using SecretSanta.Infra.Mail.SPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Mail.API.Services
{
    public class MailServices : IMailService
    {
        private readonly IMailServiceConfiguration mailServiceConfiguration;
        private readonly IMapper mapper;

        public MailServices(IMailServiceConfiguration mailServiceConfiguration)
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MailMapperProfile>());
            this.mapper = mapperConfiguration.CreateMapper();
            this.mailServiceConfiguration = mailServiceConfiguration;
        }

        public void SendReceiverIdentityToGifterByEmail(List<GiftCoupleWithEmailDto> giftCoupleWithEmailDtos)
        {
            string from = this.mailServiceConfiguration.SmtpClientUsername;

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = this.mailServiceConfiguration.SmtpServerHost;
                smtpClient.Port = this.mailServiceConfiguration.SmtpServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential
                {
                    UserName = from,
                    Password = this.mailServiceConfiguration.SmtpClientPassword
                };
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                foreach (var couple in giftCoupleWithEmailDtos)
                {
                    var email = this.mapper.Map<GiftCoupleWithEmail>(couple).GetMailObject(from);
                    smtpClient.Send(email);
                }
            }
        }
    }
}
