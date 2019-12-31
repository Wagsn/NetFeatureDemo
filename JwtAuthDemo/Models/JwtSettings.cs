using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo
{
    public class JwtSettings
    {
        public string Issuser { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; } = "JwtAuthDemo.SecretKey";
    }
}
