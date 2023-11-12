using Microsoft.Extensions.Configuration;
using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Interfaces;
using SecretSanta.Business.API.Services;
using SecretSanta.Console.Config;
using SecretSanta.Infra.Files.API.Interfaces;
using SecretSanta.Infra.Files.API.Services;
using SecretSanta.Infra.Mail.API.Interfaces;
using SecretSanta.Infra.Mail.API.Services;
using SecretSanta.Infra.Mail.SPI.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

        IMailServiceConfiguration mailServiceConfig = new MailServiceConfiguration(configuration);
        IMailService mailService = new MailServices(mailServiceConfig);
        ISecretSantaService santaService = new SecretSantaService();
        IFileService fileservice = new FileService();

        string membersFilePath = configuration["FileConfiguration:MembersFilePath"] ?? "";
        string constraintsFilePath = configuration["FileConfiguration:ConstraintsFilePath"] ?? "";
        string resultFilePath = configuration["FileConfiguration:ResultFilePath"] ?? "";

        var members = fileservice.ReadMembersFromFile(membersFilePath);
        var constraints = fileservice.ReadConstraintsFromFile(constraintsFilePath);
        
        while (true)
        {
            var resultLines = new List<string>();
            foreach (var couple in santaService.ComputeCouples(members, constraints, true))
            {
                var line = $"{couple.Gifter} must gift to {couple.Receiver}";
                Console.WriteLine(line);
                resultLines.Add(line);
            }

            fileservice.WriteLinesToFile(resultFilePath, resultLines);

            Console.ReadKey();
            Console.Clear();
        }
    }
}