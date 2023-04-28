using Microsoft.AspNetCore.Http;

namespace OpenFreeSchools.UserContext;

public interface IServerUserInfoService
{
	void ReceiveRequestHeaders(IHeaderDictionary headers);
	UserInfo? UserInfo { get; }
}