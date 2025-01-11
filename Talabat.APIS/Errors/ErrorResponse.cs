
namespace Talabat.APIS.Errors
{
	public class ErrorResponse
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }

		public ErrorResponse(int _statusCode, string? _message = null)
		{
			StatusCode = _statusCode;
			Message = _message ?? GetDefaultMessageForStatusCode(StatusCode);
		}

		private string? GetDefaultMessageForStatusCode(int statusCode)
		{
			return StatusCode switch
			{
				400 => "Bad Request",
				401 => "You Are Not Authorized",
				404 => "Resource Not Found",
				500 => "Internal Server Error",
				_ => null
			};
		}
	}
}
