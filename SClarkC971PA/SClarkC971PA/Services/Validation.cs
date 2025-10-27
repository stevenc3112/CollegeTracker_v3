using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SClarkC971PA.Services
{
    public static class Validation
    {
        public static bool ValidateDates(DateTime startingDate, DateTime endingDate)
        {
            if (startingDate < endingDate)
            {
                return true;
            }
            return false;
        }
    }
}
