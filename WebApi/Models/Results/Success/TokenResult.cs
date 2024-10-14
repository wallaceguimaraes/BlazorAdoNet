using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Models.Results.Success
{
    public class TokenResult : IActionResult
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }

        public TokenResult()
        {
            
        }

        public TokenResult(string token, DateTime date)
        {
            Token = token;
            ExpiresIn = (int)Math.Round((date - DateTime.UtcNow).TotalMinutes);
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new ObjectResult(this).ExecuteResultAsync(context);
        }
    }
}