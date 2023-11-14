using SecretSanta.Infra.Mail.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Mail.API.Interfaces
{
    public interface IMailService
    {
        public void SendReceiverIdentityToGifterByEmail(List<GiftCoupleWithEmailDto> giftCoupleWithEmailDtos, Action<string>? externalLogging = null);
    }
}
