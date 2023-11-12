using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Files.API.Model
{
    public class FileServiceException : Exception
    {
        public FileServiceException(string? message) : base(message)
        {
        }
    }
}
