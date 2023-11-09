using SecretSanta.Business.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.API.Interfaces
{
    public interface ISecretSantaService
    {
        public List<GiftCoupleDto> ComputeCouples(List<string> members, List<ConstraintDto> constraints);
    }
}
