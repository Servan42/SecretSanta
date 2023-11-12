using SecretSanta.Business.API.DTOs;
using SecretSanta.Infra.Files.API.DTOs;
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
        public List<ConstraintDto> ReadConstraintsFromFile(string filename);
        public List<string> ReadMembersFromFile(string filename);
        public List<MemberWithEmailDto> ReadMembersWithEmailFromFile(string filename);
    }
}
