using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Extensions;
using SecretSanta.Business.API.Interfaces;
using SecretSanta.Business.API.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public List<GiftCoupleDto> ComputeCouples(List<string> members, List<ConstraintDto> constraintsDto, bool cypherWithCaesarMinusOne = false)
        {
            if (members == null || members.Count < 3)
                throw new ArgumentException("Cannot run with less than three members", nameof(members));

            var constraints = InitConstraints(constraintsDto);

            var couplesDict = new Dictionary<string, GiftCoupleDto>();
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
                couplesDict = new();
                for (int i = 0; i < localMembers.Count - 1; i++)
                {
                    couplesDict.Add(localMembers[i], new GiftCoupleDto() { Gifter = localMembers[i], Receiver = localMembers[i + 1] });
                }

                isSolutionValid = true;
                foreach (var constraint in constraints)
                {
                    if (!couplesDict.Any(x => x.Value.Gifter == constraint.CannotGiftToMemberB && x.Value.Receiver == constraint.CannotReceiveFromMemberA))
                        continue;

                    isSolutionValid = false;
                    break;
                }
            }

            // Re-order from member list not to notice the cycle from reading the full list.
            var couples = new List<GiftCoupleDto>();
            foreach (var member in members)
            {
                couples.Add(couplesDict[member]);
            }

            if (cypherWithCaesarMinusOne)
                CypherWithCaesarMinusOneAndAddGarbage(couples);

            return couples;
        }

        private List<Constraints> InitConstraints(List<ConstraintDto> constraintsDto)
        {
            var constraints = new List<Constraints>();

            if (constraintsDto == null)
                return constraints;

            foreach (var constraintDto in constraintsDto)
            {
                constraints.Add(new Constraints
                {
                    CannotGiftToMemberB = constraintDto.CannotGiftToMemberB,
                    CannotReceiveFromMemberA = constraintDto.CannotReceiveFromMemberA
                });

                if (constraintDto.IsViceVersa)
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

        private void CypherWithCaesarMinusOneAndAddGarbage(List<GiftCoupleDto> couples)
        {
            foreach (var couple in couples)
            {
                couple.Receiver = couple.Receiver
                    .ToLower()
                    .Replace('é', 'e')
                    .Replace('è', 'e')
                    .Replace('à', 'a')
                    .Replace('ù', 'u')
                    .Replace('â', 'a')
                    .Replace('ê', 'e')
                    .Replace('î', 'i')
                    .Replace('ô', 'o')
                    .Replace('û', 'u')
                    .Replace('ä', 'a')
                    .Replace('ë', 'e')
                    .Replace('ï', 'i')
                    .Replace('ö', 'o')
                    .Replace('ü', 'u')
                    .Replace('ÿ', 'y')
                    .Replace('ã', 'a')
                    .Replace('õ', 'o');
                couple.Receiver = CaesarMinusOne(couple.Receiver);
            }
            int maxLength = couples.Max(x => x.Receiver.Length);
            foreach (var couple in couples)
            {
                couple.Receiver = AddGarbage(couple.Receiver, maxLength);
            }
        }

        private string AddGarbage(string receiver, int maxLength)
        {
            maxLength += 2;

            int nbCharsToAdd = maxLength - receiver.Length;
            int nbCharstoAddBefore = rng.Next(nbCharsToAdd + 1);
            int nbCharsToAddAfter = nbCharsToAdd - nbCharstoAddBefore;

            for (int i = 0; i < nbCharstoAddBefore; i++)
                receiver = (char)rng.Next('a', 'z' + 1) + receiver;

            for (int i = 0; i < nbCharsToAddAfter; i++)
                receiver = receiver + (char)rng.Next('a', 'z' + 1);

            return receiver;
        }

        private string CaesarMinusOne(string receiver)
        {
            StringBuilder cypher = new();
            foreach (var letter in receiver)
            {
                if (letter < 'a' || letter > 'z')
                    continue;

                int letterFrom0 = (int)letter - (int)'a';
                int newLetterFrom0 = letterFrom0 - 1;
                if (newLetterFrom0 > 25 || newLetterFrom0 < 0)
                    newLetterFrom0 = newLetterFrom0.Modulo(26);
                int newLetter = newLetterFrom0 + (int)'a';
                cypher.Append((char)newLetter);
            }
            return cypher.ToString();
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
