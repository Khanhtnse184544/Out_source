using System.Reflection;
using DAL.Context;
using Microsoft.AspNetCore.Mvc;

namespace GameUserServicesBackend.Controllers
{
	[ApiController]
	[Route("health")]
	public class HealthController : ControllerBase
	{
		private readonly db_userservicesContext _dbContext;

		public HealthController(db_userservicesContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var assembly = Assembly.GetExecutingAssembly().GetName();
			var result = new Dictionary<string, object?>
			{
				["status"] = "ok",
				["service"] = assembly.Name,
				["version"] = assembly.Version?.ToString(),
				["timestampUtc"] = DateTime.UtcNow.ToString("o")
			};

			bool dbUp;
			try
			{
				dbUp = await _dbContext.Database.CanConnectAsync();
			}
			catch (Exception ex)
			{
				dbUp = false;
				result["dbError"] = ex.GetType().Name + ": " + ex.Message;
			}

			result["database"] = new
			{
				up = dbUp
			};

			if (!dbUp)
			{
				result["status"] = "degraded";
				return StatusCode(503, result);
			}

			return Ok(result);
		}
	}
}


