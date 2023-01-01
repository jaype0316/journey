using Journey.Core.Models;
using Journey.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Providers
{
    public interface IEntityKeyProvider
    {
        string Provide(UserContext context);
    }

    public class DefaultEntityKeyProvider : IEntityKeyProvider
    {
        public string Provide(UserContext context)
        {
            /*
             * Multiply flt with 25 and take the Floor of the result. This will return an integer (shift) that is between 0
                and 25 and is inclusive.
                
                Add the shift integer to 65 which is the ASCII value of the character A. This will return an inclusive value between 65
                and 90, which will be the ASCII value of some character. Converting that value to a character will return an uppercase character
             * 
             */
            int length = 7;

            StringBuilder randomString = new StringBuilder();
            Random random = new Random();

            char letter;
            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                randomString.Append(letter);
            }
            

            return $"{context.UserId}_{randomString.ToString()}";
        }
    }
}
