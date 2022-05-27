using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDepartment.Common
{
    public static class Constants
    {
        public const string STUDENTROLE = "student";
        public const string MANAGERROLE = "manager";
        public const string TEACHERROLE = "teacher";
        public const string ADMINROLE = "admin";

        public static IEnumerable<string> GetRoles()
        {
            yield return STUDENTROLE;
            yield return MANAGERROLE;
            yield return TEACHERROLE;
            yield return ADMINROLE;
        }


        public class AuthOptions
        {
            public const string ISSUER = "MyAuthServer"; // издатель токена
            public const string AUDIENCE = "MyAuthClient"; // потребитель токена
            const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
            public const double LIFETIME = 300;
            public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
