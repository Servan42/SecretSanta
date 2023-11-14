using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.API.DTOs
{
    public class GiftCoupleDto
    {
        public string Gifter { get; set; }
        public string Receiver { get; set; }
        public string CypheredReceiver { get; set; }
    }
}
