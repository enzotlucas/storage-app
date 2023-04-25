using Storage.App.MVC.Core.Enterprise;
using System.Security.Claims;

namespace Storage.App.MVC.Domain.Enterprise
{
    public static class EnterpriseExtensions
    {
        public static IEnumerable<Claim> GenerateClaims(this EnterpriseEntity enterprise)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, enterprise.Id.ToString()),
                new Claim(ClaimTypes.GivenName, enterprise.Name),
                new Claim(ClaimTypes.Name, enterprise.Name),
                new Claim(ClaimTypes.MobilePhone, enterprise.PhoneNumber),
                new Claim(ClaimTypes.Email, enterprise.Email),
                new Claim("UserType","Enterprise")
            };
        }
    }
}
