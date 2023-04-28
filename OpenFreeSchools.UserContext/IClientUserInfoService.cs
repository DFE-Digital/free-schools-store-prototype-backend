using System.Security.Claims;

namespace OpenFreeSchools.UserContext;

public interface IClientUserInfoService
{
	void SetPrincipal(ClaimsPrincipal claimsPrincipal);
	void SetPrincipal(UserInfo userInfo);

	bool AddUserInfoRequestHeaders(HttpClient client);
	UserInfo UserInfo { get; }
}