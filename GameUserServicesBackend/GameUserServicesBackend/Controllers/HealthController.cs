using System.Reflection;
using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameUserServicesBackend.Controllers
{
	[ApiController]
	[Route("health")]
	public class HealthController : ControllerBase
	{
		private readonly IDbContextFactory<db_userservicesContext> _dbContextFactory;

		public HealthController(IDbContextFactory<db_userservicesContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var assembly = Assembly.GetExecutingAssembly().GetName();
			var result = new Dictionary<string, object?>
			{
				["status"] = "ok",
				["service"] = assembly.Name,
				["version"] = "1.0.0.0_[OA_7]",
				["timestampUtc"] = DateTime.UtcNow.ToString("o")
			};

			bool dbUp = false;
			string? dbError = null;
			
			// Retry mechanism for database connection
			for (int attempt = 1; attempt <= 3; attempt++)
			{
				try
				{
					using var dbContext = await _dbContextFactory.CreateDbContextAsync();
					dbUp = await dbContext.Database.CanConnectAsync();
					
					if (dbUp)
					{
						break; // Success, exit retry loop
					}
				}
				catch (Exception ex)
				{
					dbError = ex.GetType().Name + ": " + ex.Message;
					
					// Wait before retry (exponential backoff)
					if (attempt < 3)
					{
						await Task.Delay(attempt * 1000); // 1s, 2s delays
					}
				}
			}

			result["database"] = new
			{
				up = dbUp,
				error = dbError
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


