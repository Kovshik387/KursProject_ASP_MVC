﻿using System.Security.Cryptography;
using System.Text;

namespace KursProjectDataBase.Helpers
{
    public interface IHelper
    {
        public string HashString(int str);
    }
    public class HashHelper : IHelper
    {
        public string HashString(int str)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str.ToString()));
                var hash = BitConverter.ToString(hashedBytes).Replace("-","").ToLower();

                return hash;
            }
        }
    }
}
