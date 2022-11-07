using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPSTI.PlenoSoft.Core.CrossProject.Connections;
using MPSTI.PlenoSoft.Core.CrossProject.Results;
using System;

namespace MPSTI.PlenoSoft.Core.CrossProject.Web.MVC
{
	[ApiController, Route("api/1.0/[controller]/[action]")]
	public abstract class BaseControllerAPI : BaseController
	{
		public BaseControllerAPI(IConnectionManagerFactory connectionManagerFactory) : base(connectionManagerFactory) { }

		[HttpGet, AllowAnonymous]
		public virtual ActionResult<dynamic> IsAlive() => Result.Ok(base.Ok(DateTime.UtcNow)).Add("Yes, I'm alive!");
	}
}