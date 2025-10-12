using DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameUserServicesBackend.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class DatabaseTestController : ControllerBase
    {
        private readonly db_userservicesContext _dbContext;

        public DatabaseTestController(db_userservicesContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("database")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                // Test basic connection
                var canConnect = await _dbContext.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Cannot connect to database",
                        timestamp = DateTime.UtcNow.ToString("o")
                    });
                }

                // Test simple query - get count of users
                var userCount = await _dbContext.Users.CountAsync();
                
                // Test another table
                var itemCount = await _dbContext.Items.CountAsync();

                return Ok(new
                {
                    success = true,
                    message = "Database connection successful",
                    data = new
                    {
                        userCount = userCount,
                        itemCount = itemCount,
                        connectionString = _dbContext.Database.GetConnectionString()?.Substring(0, 50) + "..."
                    },
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = "Database connection failed",
                    error = new
                    {
                        type = ex.GetType().Name,
                        message = ex.Message,
                        innerException = ex.InnerException?.Message
                    },
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _dbContext.Users
                    .Take(5) // Limit to 5 users for testing
                    .Select(u => new
                    {
                        u.UserId,
                        u.UserName,
                        u.Email,
                        u.Status,
                        u.MemberTypeId
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Retrieved {users.Count} users",
                    data = users,
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = "Failed to retrieve users",
                    error = new
                    {
                        type = ex.GetType().Name,
                        message = ex.Message,
                        innerException = ex.InnerException?.Message
                    },
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var items = await _dbContext.Items
                    .Take(5) // Limit to 5 items for testing
                    .Select(i => new
                    {
                        i.ItemId,
                        i.Name,
                        i.Type,
                        i.Status
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Retrieved {items.Count} items",
                    data = items,
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = "Failed to retrieve items",
                    error = new
                    {
                        type = ex.GetType().Name,
                        message = ex.Message,
                        innerException = ex.InnerException?.Message
                    },
                    timestamp = DateTime.UtcNow.ToString("o")
                });
            }
        }
    }
}
