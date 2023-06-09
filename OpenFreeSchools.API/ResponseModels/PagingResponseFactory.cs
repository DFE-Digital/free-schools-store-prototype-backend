using Microsoft.AspNetCore.Http.Extensions;
using OpenFreeSchools.API.Contracts.ResponseModels;

namespace OpenFreeSchools.API.ResponseModels
{
    public static class PagingResponseFactory
	{
		public static PagingResponse Create(int page, int count, int recordCount, HttpRequest request)
		{
			var pagingResponse = new PagingResponse
			{
				RecordCount = recordCount,
				Page = page
			};

			if ((count * page) >= recordCount) return pagingResponse;

			var queryAttributes = request.Query
				.Where(q => q.Key != nameof(page) && q.Key != nameof(count))
				.Select(q => new KeyValuePair<string, string>(q.Key, q.Value));

			var queryBuilder = new QueryBuilder(queryAttributes)
			{
				{nameof(page), $"{page + 1}"}, 
				{nameof(count), $"{count}"}
			};

			pagingResponse.NextPageUrl = $"{request.Path}{queryBuilder}";

			return pagingResponse;
		}
	}
}