using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server
{
    public static class Constants
    {
        public const string Issuer = Audiance;
        public const string Audiance = "https://localhost:5005/";
      

        public const string Secret = "not_too_short_secret_otherwise_it_might_error";

    }

}
