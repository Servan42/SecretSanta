using SecretSanta.Business.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.API.Extensions
{
    internal static class QualityOfLifeExtensions
    {
        internal static bool ContainsTheConstraint(this Dictionary<string, GiftCouple> couplesDict, Constraints constraint)
        {
            return couplesDict.Any(
                x => x.Value.Gifter.ToLower() == constraint.CannotGiftToMemberB.ToLower()
                && x.Value.Receiver.ToLower() == constraint.CannotReceiveFromMemberA.ToLower());
        }
    }
}
