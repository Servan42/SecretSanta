using SecretSanta.Business.API.DTOs;
using SecretSanta.Infra.Files.API.Interfaces;
using SecretSanta.Infra.Files.API.Model;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Files.API.Services
{
    public class FileService : IFileService
    {
        private readonly IFileSystem fileSystem;

        private const string CONSTRAINTS_HEADERS = "CannotGiftToMemberB,CannotReceiveFromMemberA,IsViceVersa";

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
            return this.fileSystem.File.ReadAllLines(filename).ToList();
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
                if (string.IsNullOrEmpty(headers) || headers != CONSTRAINTS_HEADERS)
                    throw new FileServiceException($"{filename} was not recognized as a constraint file. It must contain CSV headers {CONSTRAINTS_HEADERS}");

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
    }
}
