using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<GiftCoupleDto> ComputeCouples(List<string> members, List<ConstraintDto> constraints)
        {
            var localMembers = members.ToList();
            Shuffle(localMembers);
            localMembers.Add(localMembers[0]);
            var couples = new List<GiftCoupleDto>();
            for (int i = 0; i < localMembers.Count - 1; i++)
            {
                couples.Add(new GiftCoupleDto() { Gifter = localMembers[i], Receiver = localMembers[i + 1] });
            }
            return couples;
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
