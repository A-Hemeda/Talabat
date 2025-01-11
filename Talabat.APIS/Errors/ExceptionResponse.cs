namespace Talabat.APIS.Errors
{
	public class ExceptionResponse : ErrorResponse
	{
		public string? Details { get; set; }

		public ExceptionResponse(int _statusCode, string? _message = null, string? _details = null) : base(_statusCode, _message)
		{
			Details = _details;
		}
	}
}
