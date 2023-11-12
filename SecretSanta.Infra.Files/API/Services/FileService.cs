using SecretSanta.Business.API.DTOs;
using SecretSanta.Infra.Files.API.DTOs;
using SecretSanta.Infra.Files.API.Interfaces;
using SecretSanta.Infra.Files.API.Model;
using System.IO.Abstractions;
using System.Net.Mail;

namespace SecretSanta.Infra.Files.API.Services
{
    public class FileService : IFileService
    {
        private readonly IFileSystem fileSystem;

        private const string CONSTRAINTS_HEADER = "CannotGiftToMemberB,CannotReceiveFromMemberA,IsViceVersa";
        private const string MEMBERS_WITH_EMAIL_HEADER = "member,email";

        public FileService()
        {
            fileSystem = new FileSystem();
        }

        public FileService(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public List<string> ReadMembersFromFile(string filename)
        {
            var result = new List<string>();
            string? line;

            using (StreamReader sr = this.fileSystem.File.OpenText(filename))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    result.Add(line.Trim());
                }
            }
            return result;
        }


        public void WriteLinesToFile(string filename, List<string> lines)
        {
            using (StreamWriter sw = this.fileSystem.File.CreateText(filename))
            {
                lines.ForEach(line => sw.WriteLine(line));
            }
        }

        public List<ConstraintDto> ReadConstraintsFromFile(string filename)
        {
            var constraints = new List<ConstraintDto>();
            string? line;

            using (StreamReader sr = this.fileSystem.File.OpenText(filename))
            {
                string? headers = sr.ReadLine();
                if (string.IsNullOrEmpty(headers) || headers != CONSTRAINTS_HEADER)
                    throw new FileServiceException($"{filename} was not recognized as a constraint file. It must contain CSV headers {CONSTRAINTS_HEADER}");

                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    var splittedLine = line.Split(',');

                    if(splittedLine.Length != 3)
                        throw new FileServiceException($"{filename} contains an unrecognized line: {line}");

                    constraints.Add(new ConstraintDto
                    {
                        CannotGiftToMemberB = splittedLine[0].Trim(),
                        CannotReceiveFromMemberA = splittedLine[1].Trim(),
                        IsViceVersa = bool.Parse(splittedLine[2].Trim())
                    });
                }
            }

            return constraints;
        }

        public List<MemberWithEmailDto> ReadMembersWithEmailFromFile(string filename)
        {
            var memberWithEmail = new List<MemberWithEmailDto>();
            string? line;

            using (StreamReader sr = this.fileSystem.File.OpenText(filename))
            {
                string? headers = sr.ReadLine();
                if (string.IsNullOrEmpty(headers) || headers != MEMBERS_WITH_EMAIL_HEADER)
                    throw new FileServiceException($"{filename} was not recognized as a membersWithEmail file. It must contain CSV headers {MEMBERS_WITH_EMAIL_HEADER}");

                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    var splittedLine = line.Split(',');

                    if (splittedLine.Length != 2)
                        throw new FileServiceException($"{filename} contains an unrecognized line: {line}");

                    memberWithEmail.Add(new MemberWithEmailDto
                    {
                        Member = splittedLine[0].Trim(),
                        Email = new MailAddress(splittedLine[1].Trim())
                    });
                }
            }

            return memberWithEmail;
        }
    }
}
