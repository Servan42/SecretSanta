using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.API.Model
{
    public class Constraints
    {
        public string CannotGiftToMemberB { get; set; }
        public string CannotReceiveFromMemberA { get; set; }
    }
}
