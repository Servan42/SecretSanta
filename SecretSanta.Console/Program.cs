using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Extensions;
using SecretSanta.Business.API.Services;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var service = new SecretSantaService();

        while (true)
        {
            var members = new List<string>
            {
                "Servan",
                "Coline",
                "Eva",
                "Patrice",
                "Fred",
                "Rose"
            };

            var constraints = new List<ConstraintDto>
            {

            };

            foreach (var couple in service.ComputeCouples(members, constraints, true))
            {
                Console.WriteLine($"{couple.Gifter} must gift to {couple.Receiver}");
            }

            Console.ReadKey();
            Console.Clear();
        }
    }
}