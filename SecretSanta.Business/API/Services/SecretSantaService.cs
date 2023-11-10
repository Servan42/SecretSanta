using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Interfaces;
using SecretSanta.Business.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.API.Services
{
    public class SecretSantaService : ISecretSantaService
    {
        private readonly Random rng;

        public SecretSantaService()
        {
            rng = new Random();
        }

        public SecretSantaService(int seed)
        {
            rng = new Random(seed);
        }

        public List<GiftCoupleDto> ComputeCouples(List<string> members, List<ConstraintDto> constraintsDto)
        {
            var constraints = InitConstraints(constraintsDto);

            var couples = new List<GiftCoupleDto>();
            bool isSolutionValid = false;
            int retryCount = -1;

            while (!isSolutionValid)
            {
                retryCount++;
                if (retryCount > 1000000)
                    throw new BusinessException("Could not find a solution. Try removing constraints or adding members.");

                var localMembers = members.ToList();
                Shuffle(localMembers);
                localMembers.Add(localMembers[0]);
                couples = new List<GiftCoupleDto>();
                for (int i = 0; i < localMembers.Count - 1; i++)
                {
                    couples.Add(new GiftCoupleDto() { Gifter = localMembers[i], Receiver = localMembers[i + 1] });
                }

                isSolutionValid = true;
                foreach (var constraint in constraints)
                {
                    if (!couples.Any(x => x.Gifter == constraint.CannotGiftToMemberB && x.Receiver == constraint.CannotReceiveFromMemberA))
                        continue;

                    isSolutionValid = false;
                    break;
                }
            }

            return couples;
        }

        private List<Constraints> InitConstraints(List<ConstraintDto> constraintsDto)
        {
            var constraints = new List<Constraints>();
            foreach(var constraintDto in constraintsDto)
            {
                constraints.Add(new Constraints 
                { 
                    CannotGiftToMemberB = constraintDto.CannotGiftToMemberB, 
                    CannotReceiveFromMemberA = constraintDto.CannotReceiveFromMemberA 
                });

                if(constraintDto.IsViceVersa)
                {
                    constraints.Add(new Constraints
                    {
                        CannotGiftToMemberB = constraintDto.CannotReceiveFromMemberA,
                        CannotReceiveFromMemberA = constraintDto.CannotGiftToMemberB
                    });
                }
            }
            return constraints;
        }

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
