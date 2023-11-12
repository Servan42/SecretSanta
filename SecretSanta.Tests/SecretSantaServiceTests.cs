using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Interfaces;
using SecretSanta.Business.API.Model;
using SecretSanta.Business.API.Services;
using System.Numerics;

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
            Assert.That(NoOneToOneExchanges(couples), "Should not 1v1 exchange");
            Assert.That(EveryoneGiftsAndEveryoneGetsAGift(members, couples), "Everyone should gift and receive a gift");
        }

        [Test]
        public void Should_create_gift_couples_with_special_constraint()
        {
            // GIVEN
            secretSantaService_sut = new SecretSantaService(seed: 0); // A -> B -> D -> C -> A
            var members = new List<string>() { "A", "B", "c", "D" };

            // WHEN
            var couples = secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto> { new ConstraintDto { CannotGiftToMemberB = "d", CannotReceiveFromMemberA = "C" } });

            // THEN
            Assert.That(couples.Count, Is.EqualTo(4));
            Assert.That(couples.All(x => x.Gifter != x.Receiver), Is.True, "Should not gift to self");
            Assert.That(NoOneToOneExchanges(couples), "Should not 1v1 exchange");
            Assert.That(EveryoneGiftsAndEveryoneGetsAGift(members, couples), "Everyone should gift and receive a gift");
            
            Assert.That(couples.Any(x => x.Gifter == "D" && x.Receiver == "c"), Is.False, "D should not gift to C");
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

        [Test]
        [Timeout(10000)]
        public void Should_throw_exception_when_no_solution_could_be_found()
        {
            // GIVEN
            var members = new List<string>() { "A", "B", "C" };
            var constraints = new List<ConstraintDto>
            {
                new ConstraintDto { CannotGiftToMemberB = "A", CannotReceiveFromMemberA = "B"},
                new ConstraintDto { CannotGiftToMemberB = "B", CannotReceiveFromMemberA = "A"}
            };

            // WHEN
            var ex = Assert.Throws<BusinessException>(() => secretSantaService_sut.ComputeCouples(members, constraints));

            Assert.That(ex.Message, Is.EqualTo("Could not find a solution. Try removing constraints or adding members."));
        }

        [Test]
        [Timeout(10000)]
        public void Should_throw_exception_when_no_solution_could_be_found_viceversa_constraint()
        {
            // GIVEN
            var members = new List<string>() { "A", "B", "C" };
            var constraints = new List<ConstraintDto>
            {
                new ConstraintDto { CannotGiftToMemberB = "A", CannotReceiveFromMemberA = "B", IsViceVersa = true }
            };

            // WHEN
            var ex = Assert.Throws<BusinessException>(() => secretSantaService_sut.ComputeCouples(members, constraints));

            Assert.That(ex.Message, Is.EqualTo("Could not find a solution. Try removing constraints or adding members."));
        }

        [Test]
        public void Should_throw_exception_when_is_given_less_than_three_members()
        {
            // GIVEN
            var members = new List<string>() { "A", "B" };

            // WHEN
            var ex = Assert.Throws<ArgumentException>(() => secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto>()));

            Assert.That(ex.Message, Is.EqualTo("Cannot run with less than three members (Parameter 'members')"));
        }

        [Test]
        public void Should_throw_exception_when_is_given_a_list_of_non_unique_members()
        {
            // GIVEN
            var members = new List<string>() { "A", "B", "A" };

            // WHEN
            var ex = Assert.Throws<ArgumentException>(() => secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto>()));

            Assert.That(ex.Message, Is.EqualTo("Cannot run with a list of non-unique members (Parameter 'members')"));
        }

        [Test]
        public void Should_cypher_receivers_name_with_caesar_minus_one_and_add_garbage()
        {
            // GIVEN
            var members = new List<string>() { "Alice", "Bob", "ÈË‡˘‚ÍÓÙ‘- ˚‰ÎÔˆ¸ˇ„ı" };
            secretSantaService_sut = new SecretSantaService(seed: 0);

            // WHEN
            var couples = secretSantaService_sut.ComputeCouples(members, new List<ConstraintDto>(), true);

            // THEN
            Assert.That(couples[0].Receiver, Is.EqualTo("azmqmhhzlxofoanawzri")); // garbage + ana + garbage
            Assert.That(couples[1].Receiver, Is.EqualTo("zwddztzdhnntzdhntxzn")); // garbage + ddztzdhnntzdhntxzn + garbage
            Assert.That(couples[2].Receiver, Is.EqualTo("zkhbdsnyrocelhzqtaji")); // garbage + zkhbd + garbage
        }

        #region Helpers
        private bool EveryoneGiftsAndEveryoneGetsAGift(List<string> members, List<GiftCoupleDto> giftCouples)
        {
            foreach (var member in members)
            {
                if (giftCouples.Count(x => x.Gifter == member) != 1)
                    return false;
                if (giftCouples.Count(x => x.Receiver == member) != 1)
                    return false;
            }
            return true;
        }

        private bool NoOneToOneExchanges(List<GiftCoupleDto> giftCouples)
        {
            foreach (var couple in giftCouples)
            {
                if (giftCouples.Any(x => x.Gifter == couple.Receiver && x.Receiver == couple.Gifter))
                    return false;
            }
            return true;
        } 
        #endregion
    }
}