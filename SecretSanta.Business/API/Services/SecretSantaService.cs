using AutoMapper;
using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Extensions;
using SecretSanta.Business.API.Interfaces;
using SecretSanta.Business.API.MapperProfile;
using SecretSanta.Business.API.Model;

namespace SecretSanta.Business.API.Services
{
    public class SecretSantaService : ISecretSantaService
    {
        private readonly IMapper mapper;
        private readonly Random randomNumberGenerator;

        public SecretSantaService()
        {
            randomNumberGenerator = new Random();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<BusinessMapperProfile>());
            mapper = mapperConfig.CreateMapper();
        }

        public SecretSantaService(int seed)
        {
            randomNumberGenerator = new Random(seed);
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<BusinessMapperProfile>());
            mapper = mapperConfig.CreateMapper();
        }

        public List<GiftCoupleDto> ComputeCouples(List<string> members, List<ConstraintDto> constraintsDto, bool cypherWithCaesarMinusOne = false)
        {
            if (members == null || members.Count < 3)
                throw new ArgumentException("Cannot run with less than three members", nameof(members));

            List<Constraints> constraints = MapConstraints(constraintsDto);

            var couplesDict = new Dictionary<string, GiftCouple>();
            bool isSolutionValid = false;
            int retryCount = -1;

            while (!isSolutionValid)
            {
                retryCount++;
                if (retryCount > 1000000)
                    throw new BusinessException("Could not find a solution. Try removing constraints or adding members.");

                var localMembers = members.ToList();
                localMembers.Shuffle(this.randomNumberGenerator);
                localMembers.Add(localMembers[0]);
                couplesDict = new();
                
                for (int i = 0; i < localMembers.Count - 1; i++)
                {
                    couplesDict.Add(localMembers[i], new GiftCouple() { Gifter = localMembers[i], Receiver = localMembers[i + 1] });
                }

                isSolutionValid = true;
                foreach (var constraint in constraints)
                {
                    if (couplesDict.ContainsTheConstraint(constraint))
                    {
                        isSolutionValid = false;
                        break;
                    }
                }
            }

            if (cypherWithCaesarMinusOne)
                CypherWithCaesarMinusOneAndAddGarbage(couplesDict);

            List<GiftCoupleDto> couples = ReorderAlongInputMemberAndMapToOutput(members, couplesDict);

            return couples;
        }

        /// <summary>
        /// Re-order from member list not to notice the cycle from reading the full list.
        /// </summary>
        private List<GiftCoupleDto> ReorderAlongInputMemberAndMapToOutput(List<string> members, Dictionary<string, GiftCouple> couplesDict)
        {
            var couples = new List<GiftCoupleDto>();

            foreach (var member in members)
            {
                couples.Add(this.mapper.Map<GiftCoupleDto>(couplesDict[member]));
            }

            return couples;
        }

        private List<Constraints> MapConstraints(List<ConstraintDto> constraintsDto)
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

        private void CypherWithCaesarMinusOneAndAddGarbage(Dictionary<string, GiftCouple> couples)
        {
            foreach (var couple in couples)
            {
                couple.Value.CleanAndLowerReceiver();
                couple.Value.CaesarMinusOneReceiver();
            }

            int maxLength = couples.Max(x => x.Value.Receiver.Length);
            foreach (var couple in couples)
            {
                couple.Value.AddGarbageToReceiver(maxLength, this.randomNumberGenerator);
            }
        }
    }
}
