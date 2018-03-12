using System;
using System.Text;


namespace Umi.Core
{
    public class BasicAuthentication
    {

        public BasicAuthentication(string user, string password)
        {
            this.User = user;
            this.Password = password;
        }

        public string Password { get; }
        public string User { get; }

        public string AsToken => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{User}:{Password}"));

    }
}