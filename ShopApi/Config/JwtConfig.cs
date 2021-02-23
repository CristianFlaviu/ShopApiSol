using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Config
{
    public class JwtConfig
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public string ExpireDays { get; set; }
        public string Audience { get; set; }
    }
}
