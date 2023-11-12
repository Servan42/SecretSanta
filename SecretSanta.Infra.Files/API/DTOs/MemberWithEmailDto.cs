using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Infra.Files.API.DTOs
{
    public class MemberWithEmailDto
    {
        public string Member { get; set; }
        public MailAddress Email { get; set; }
    }
}
