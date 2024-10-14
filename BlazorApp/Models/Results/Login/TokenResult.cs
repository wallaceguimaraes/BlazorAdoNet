using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models.Results.Login
{
    public class TokenResult
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}