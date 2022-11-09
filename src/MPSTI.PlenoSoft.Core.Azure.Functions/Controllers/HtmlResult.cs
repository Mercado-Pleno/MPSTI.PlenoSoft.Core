using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Controllers
{
	public class HtmlResult : ContentResult
	{
		public HtmlResult(string contentType, string content, int statusCode = StatusCodes.Status200OK)
		{
			Content = content;
			StatusCode = statusCode;
			ContentType = contentType;
		}
		public HtmlResult(string htmlBody, int statusCode = StatusCodes.Status200OK)
			: this("text/html; charset=utf-8", htmlBody, statusCode) { }

	}
}