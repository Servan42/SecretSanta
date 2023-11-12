using NUnit.Framework;
using SecretSanta.Infra.Mail.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Tests
{
    internal class MailServiceTests
    {
        [Test]
        public void Should_create_mailobjects_from_input_dto()
        {
            // GIVEN
            GiftCoupleWithEmail giftCoupleWithEmail_sut = new GiftCoupleWithEmail
            {
                Gifter = "Alice",
                GifterEmail = "alice@mail.com",
                Receiver = "Bob"
            };

            // WHEN
            var mailObject = giftCoupleWithEmail_sut.GetMailObject("sender@mail.com");

            // THEN
            Assert.That(mailObject.From.Address, Is.EqualTo("sender@mail.com"));
            Assert.That(mailObject.To.First().Address, Is.EqualTo("alice@mail.com"));
            Assert.That(mailObject.Subject, Is.EqualTo("Secret Santa"));
            Assert.That(mailObject.Body, Is.EqualTo("Hello Alice, for this year's secret santa, you must prepare a present for Bob.\n\nThis message was sent by a program. Source: https://github.com/Servan42/SecretSanta"));
        }
    }
}
