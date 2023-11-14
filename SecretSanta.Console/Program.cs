using Microsoft.Extensions.Configuration;
using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Interfaces;
using SecretSanta.Business.API.Services;
using SecretSanta.Console.Config;
using SecretSanta.Infra.Files.API.DTOs;
using SecretSanta.Infra.Files.API.Interfaces;
using SecretSanta.Infra.Files.API.Services;
using SecretSanta.Infra.Mail.API.DTOs;
using SecretSanta.Infra.Mail.API.Interfaces;
using SecretSanta.Infra.Mail.API.Services;
using SecretSanta.Infra.Mail.SPI.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

            IMailServiceConfiguration mailServiceConfig = new MailServiceConfiguration(configuration);
            IMailService mailService = new MailServices(mailServiceConfig);
            ISecretSantaService santaService = new SecretSantaService();
            IFileService fileservice = new FileService();

            bool encypherLocalResult = bool.Parse(configuration["GeneralConfiguration:EncypherLocalResult"] ?? "true");
            bool sendResultsByEmail = bool.Parse(configuration["GeneralConfiguration:SendResultsByEmail"] ?? "false");

            string constraintsFilePath = configuration["FileConfiguration:ConstraintsFilePath"] ?? "";
            string resultFilePath = configuration["FileConfiguration:ResultFilePath"] ?? "";

            var constraints = fileservice.ReadConstraintsFromFile(constraintsFilePath);
            List<string> members;
            List<MemberWithEmailDto> membersWithEmail = new();

            if (sendResultsByEmail)
            {
                string membersWithEmailFilePath = configuration["FileConfiguration:MembersWithEmailFilePath"] ?? "";
                membersWithEmail = fileservice.ReadMembersWithEmailFromFile(membersWithEmailFilePath);
                members = membersWithEmail.Select(x => x.Member).ToList();

                Console.WriteLine("Members found: Please review the list before pressing a key to continue (emails will be sent to this list).");
                membersWithEmail.ForEach(x => Console.WriteLine($"* {x.Member} -> {x.Email}"));
                Console.ReadKey();
            }
            else
            {
                string membersFilePath = configuration["FileConfiguration:MembersFilePath"] ?? "";
                members = fileservice.ReadMembersFromFile(membersFilePath);
            }

            var giftCouples = santaService.ComputeCouples(members, constraints);

            var resultLines = new List<string>();
            foreach (var couple in giftCouples)
            {
                string receiver = encypherLocalResult ? couple.CypheredReceiver : couple.Receiver;
                var line = $"{couple.Gifter} must gift to {receiver}";
                if (!sendResultsByEmail)
                    Console.WriteLine(line);
                resultLines.Add(line);
            }

            fileservice.WriteLinesToFile(resultFilePath, resultLines);
            Console.WriteLine($"\nResults saved to {resultFilePath} (overridden) for logging purposes.");

            if (sendResultsByEmail)
            {
                var giftCoupleWithEmail = giftCouples
                    .Join(membersWithEmail, c => c.Gifter, m => m.Member, (c, m) => new GiftCoupleWithEmailDto
                    {
                        Gifter = c.Gifter,
                        GifterEmail = m.Email,
                        Receiver = c.Receiver
                    })
                    .ToList();

                mailService.SendReceiverIdentityToGifterByEmail(giftCoupleWithEmail, (string s) => Console.Write(s));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\nAN ERROR HAS OCCURED:\n\n{ex}");
        }

        Console.WriteLine("\nExecution is over, press any key to close");
        Console.ReadKey();
    }
}