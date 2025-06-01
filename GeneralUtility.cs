using System.Security.Claims;
using System.Text;



namespace PersonalERP
{
    public static class GeneralUtility
    {
        private static readonly Random random = new Random();


        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public static string GetCurrentUserName(IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext?.User ?? throw new Exception("User not found, Please try again");
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? throw new Exception("Please Login to continue");
        }

       
        public class TokenInfoDto
        {
            public string? UserName { get; set; }
            public string? OrganizationCode { get; set; }
            public string? UserType { get; set; }
            public string[]? Roles { get; set; }
        }

        public static TokenInfoDto extractInfoFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var tokenInfo = new TokenInfoDto();
            if (jwtToken != null)
            {
                tokenInfo.UserName = jwtToken.Subject;

                tokenInfo.Roles = jwtToken.Claims
                    .Where(claim => claim.Type == ClaimTypes.Role)
                    .Select(claim => claim.Value)
                    .ToArray();

                var organizationClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "OrganizationCode");
                if (organizationClaim != null)
                {
                    string code = organizationClaim.Value;
                    tokenInfo.OrganizationCode = code;
                }

                var userTypeClaim = jwtToken.Claims
                    .FirstOrDefault(claim => claim.Type == "UserType");
                if (userTypeClaim != null)
                {
                    tokenInfo.UserType = userTypeClaim.Value;
                }

                return tokenInfo;
            }
            return tokenInfo;
        }

    }
}
