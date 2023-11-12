using SecretSanta.Infra.Files.API.Interfaces;
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

        public FileService()
        {
            fileSystem = new FileSystem();
        }

        public FileService(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public List<string> ReadLinesFromFile(string filename)
        {
            return this.fileSystem.File.ReadAllLines(filename).ToList();
        }

        public void WriteLinesToFile(string filename, List<string> lines)
        {
            using(StreamWriter sw = this.fileSystem.File.CreateText(filename))
            {
                lines.ForEach(line => sw.WriteLine(line));
            }
        }
    }
}
