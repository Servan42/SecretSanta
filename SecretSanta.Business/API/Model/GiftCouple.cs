using SecretSanta.Business.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Business.API.Model
{
    public class GiftCouple
    {
        public string Gifter { get; set; }
        public string Receiver { get; set; }

        public void CleanAndLowerReceiver()
        {
            this.Receiver = this.Receiver
                    .ToLower()
                    .Replace('é', 'e')
                    .Replace('è', 'e')
                    .Replace('à', 'a')
                    .Replace('ù', 'u')
                    .Replace('â', 'a')
                    .Replace('ê', 'e')
                    .Replace('î', 'i')
                    .Replace('ô', 'o')
                    .Replace('û', 'u')
                    .Replace('ä', 'a')
                    .Replace('ë', 'e')
                    .Replace('ï', 'i')
                    .Replace('ö', 'o')
                    .Replace('ü', 'u')
                    .Replace('ÿ', 'y')
                    .Replace('ã', 'a')
                    .Replace('õ', 'o');
        }

        public void CaesarMinusOneReceiver()
        {
            StringBuilder cypher = new();
            foreach (var letter in this.Receiver)
            {
                if (letter < 'a' || letter > 'z')
                    continue;

                int letterFrom0 = (int)letter - (int)'a';
                int newLetterFrom0 = letterFrom0 - 1;
                if (newLetterFrom0 > 25 || newLetterFrom0 < 0)
                    newLetterFrom0 = newLetterFrom0.Modulo(26);
                int newLetter = newLetterFrom0 + (int)'a';
                cypher.Append((char)newLetter);
            }
            this.Receiver = cypher.ToString();
        }

        public void AddGarbageToReceiver(int maxLength, Random rng)
        {
            maxLength += 2;

            int nbCharsToAdd = maxLength - this.Receiver.Length;
            int nbCharstoAddBefore = rng.Next(nbCharsToAdd + 1);
            int nbCharsToAddAfter = nbCharsToAdd - nbCharstoAddBefore;

            for (int i = 0; i < nbCharstoAddBefore; i++)
                this.Receiver = (char)rng.Next('a', 'z' + 1) + this.Receiver;

            for (int i = 0; i < nbCharsToAddAfter; i++)
                this.Receiver = this.Receiver + (char)rng.Next('a', 'z' + 1);
        }
    }
}
