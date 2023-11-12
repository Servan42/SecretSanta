using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Files.API.Interfaces
{
    public interface IFileService
    {
        public void WriteLinesToFile(string filename, List<string> lines);

        public List<string> ReadLinesFromFile(string filename);
    }
}
