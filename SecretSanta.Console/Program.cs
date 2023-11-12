using SecretSanta.Business.API.DTOs;
using SecretSanta.Business.API.Services;
using SecretSanta.Infra.Files.API.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var santaService = new SecretSantaService();
        var fileservice = new FileService();

        while (true)
        {
            var members = fileservice.ReadMembersFromFile(@"./InputOutputFiles/memberList.txt");
            var constraints = fileservice.ReadConstraintsFromFile(@"./InputOutputFiles/constraints.csv");

            var resultLines = new List<string>();
            foreach (var couple in santaService.ComputeCouples(members, constraints, true))
            {
                var line = $"{couple.Gifter} must gift to {couple.Receiver}";
                Console.WriteLine(line);
                resultLines.Add(line);
            }

            fileservice.WriteLinesToFile(@"./InputOutputFiles/output.txt", resultLines);

            Console.ReadKey();
            Console.Clear();
        }
    }
}