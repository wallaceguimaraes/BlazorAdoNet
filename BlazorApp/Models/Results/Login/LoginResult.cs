using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Models.Results.Login
{
    public class LoginResult : Result
    {
        public string Token { get; set; }
    }
}