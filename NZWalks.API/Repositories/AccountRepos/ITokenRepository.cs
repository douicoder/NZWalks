using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories.AccountRepos
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> role);
    }
}
