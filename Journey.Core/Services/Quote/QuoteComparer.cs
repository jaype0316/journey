using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Quote
{
    public class QuoteComparer : IEqualityComparer<Journey.Models.DTO.Quote>
    {
        public bool Equals(Journey.Models.DTO.Quote? x, Journey.Models.DTO.Quote? y)
        {
            if (x == null && y == null) return true;

            return x.Text == y.Text;
        }

        public int GetHashCode([DisallowNull] Journey.Models.DTO.Quote obj)
        {
            //https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                // Suitable nullity checks etc, of course :)
                hash = (hash * 16777619) ^ obj.Text.GetHashCode();
                return hash;
            }
        }
    }
}
