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
        public string CypheredReceiver { get; set; }


        public void CleanAndLowerReceiverToCypheredReceiver()
        {
            this.CypheredReceiver = this.Receiver
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

        public void CaesarMinusOneCypheredReceiver()
        {
            StringBuilder cypher = new();
            foreach (var letter in this.CypheredReceiver)
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
            this.CypheredReceiver = cypher.ToString();
        }

        public void AddGarbageToCypheredReceiver(int maxLength, Random rng)
        {
            maxLength += 2;

            int nbCharsToAdd = maxLength - this.CypheredReceiver.Length;
            int nbCharstoAddBefore = rng.Next(nbCharsToAdd + 1);
            int nbCharsToAddAfter = nbCharsToAdd - nbCharstoAddBefore;

            for (int i = 0; i < nbCharstoAddBefore; i++)
                this.CypheredReceiver = (char)rng.Next('a', 'z' + 1) + this.CypheredReceiver;

            for (int i = 0; i < nbCharsToAddAfter; i++)
                this.CypheredReceiver = this.CypheredReceiver + (char)rng.Next('a', 'z' + 1);
        }
    }
}
