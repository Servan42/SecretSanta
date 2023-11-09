using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Interfaces;
using SecretSanta.Business.API.Services;

namespace SecretSanta.Tests
{
    public class SecretSantaServiceTests
    {
        private ISecretSantaService secretSantaService_sut;

        [SetUp]
        public void Setup()
        {
            secretSantaService_sut = new SecretSantaService();
        }


        [Test]
        public void Should_create_gift_couples_without_special_constraints()
        {
            // GIVEN
            var members = new List<string>() { "A", "B", "C", "D" };

            // WHEN
            var couples = secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto>());

            // THEN
            Assert.That(couples.Count, Is.EqualTo(4));
            Assert.That(couples.All(x => x.Gifter != x.Receiver), Is.True, "Should not gift to self");
            Assert.True(NoOneToOneExchanges(couples), "Should not 1v1 exchange");
        }

        [Test]
        public void Should_not_have_the_same_couples_when_the_seed_is_different()
        {
            // GIVEN
            var members = new List<string>() { "A", "B", "C", "D", "E", "F" };

            // WHEN
            secretSantaService_sut = new SecretSantaService(seed: 0);
            var couples1 = secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto>());
            secretSantaService_sut = new SecretSantaService(seed: 1);
            var couples2 = secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto>());

            // THEN
            string couples1string = string.Join(',', couples1.Select(x => x.Gifter + x.Receiver));
            string couples2string = string.Join(',', couples2.Select(x => x.Gifter + x.Receiver));

            Assert.That(couples1string, Is.Not.EqualTo(couples2string));
        }

        [Test]
        public void Should_have_the_same_couples_when_the_seed_is_the_same()
        {
            // GIVEN
            secretSantaService_sut = new SecretSantaService(0);
            var members = new List<string>() { "A", "B", "C", "D", "E", "F" };

            // WHEN
            secretSantaService_sut = new SecretSantaService(seed: 0);
            var couples1 = secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto>());
            secretSantaService_sut = new SecretSantaService(seed: 0);
            var couples2 = secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto>());

            // THEN
            string couples1string = string.Join(',', couples1.Select(x => x.Gifter + x.Receiver));
            string couples2string = string.Join(',', couples2.Select(x => x.Gifter + x.Receiver));

            Assert.That(couples1string, Is.EqualTo(couples2string));
        }

        private bool NoOneToOneExchanges(List<GiftCoupleDto> giftCouples)
        {
            foreach(var couple in giftCouples)
            {
                if (giftCouples.Any(x => x.Gifter == couple.Receiver && x.Receiver == couple.Gifter))
                    return false;
            }
            return true;
        }
    }
}