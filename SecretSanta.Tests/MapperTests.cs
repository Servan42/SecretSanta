using AutoMapper;
using SecretSanta.Business.API.MapperProfile;
using SecretSanta.Infra.Mail.API.MapperProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Tests
{
    internal class MapperTests
    {
        [Test]
        public void Business_Mapper_configuration_is_valid()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<BusinessMapperProfile>());
            mapperConfiguration.AssertConfigurationIsValid();
        }

        [Test]
        public void Mail_Mapper_configuration_is_valid()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MailMapperProfile>());
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
