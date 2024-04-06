using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Settings
{
    public class CORSSettings
    {
        public bool AllowAnyOrigin { get; set; }
        public string[] AllowedOrigins { get; set; }
    }
}
