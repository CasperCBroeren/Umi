using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Umi.Core.Authentication
{
    public class BasicAuthentication : IAuthentication
    {

        public BasicAuthentication(string user, string password)
        {
            this.User = user;
            this.Password = password;
        }

        public string Password { get; }
        public string User { get; }

        public string AsToken => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{User}:{Password}"));

        public bool IsAuthenticated(HttpRequest request)
        {
            return request.Headers["Authorization"].Equals($"Basic {AsToken}"); 
        }

        public bool TriesToAuthenticate(HttpRequest request)
        {
            return request.Headers.ContainsKey("Authorization");
        }
    }
}