using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Services;

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
                new ConstraintDto { CannotGiftToMemberB = "Fred", CannotReceiveFromMemberA = "Rose", IsViceVersa = true},
                new ConstraintDto { CannotGiftToMemberB = "Patrice", CannotReceiveFromMemberA = "Eva", IsViceVersa = true}
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