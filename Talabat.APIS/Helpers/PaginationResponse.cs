using Talabat.Core.Models;

namespace Talabat.APIS.Helpers
{
	public class PaginationResponse<T>
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public IReadOnlyList<T> Data { get; set; }

		public PaginationResponse(int pageNumber, int pageSize, int totalCount, IReadOnlyList<T> data)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			TotalCount = totalCount;
			Data = data;
		}
	}
}
