using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameUserServicesBackend.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly GameDataService _gameDataService;
        private readonly ILogger<GameController> _logger;

        public GameController(GameDataService gameDataService, ILogger<GameController> logger)
        {
            _gameDataService = gameDataService;
            _logger = logger;
        }

        /// <summary>
        /// Save Planted Trees
        /// </summary>
        [HttpPost("save-planted-trees")]
        public async Task<IActionResult> SavePlantedTrees([FromBody] SavePlantedTreesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _gameDataService.SavePlantedTreesAsync(request, cancellationToken);
                
                if (result == "Success")
                {
                    return Ok(new { 
                        status = "success", 
                        message = "Trees saved successfully",
                        data = new { savedCount = request.Trees.Count }
                    });
                }
                
                return BadRequest(new { status = "error", message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving planted trees for user {UserId}", request.UserId);
                return StatusCode(500, new { status = "error", message = "Lỗi lưu cây trồng" });
            }
        }

        /// <summary>
        /// Get Planted Trees
        /// </summary>
        [HttpGet("planted-trees/{userId}")]
        public async Task<IActionResult> GetPlantedTrees(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var trees = await _gameDataService.GetPlantedTreesAsync(userId, cancellationToken);
                return Ok(new { 
                    status = "success", 
                    data = new { userId, trees }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting planted trees for user {UserId}", userId);
                return StatusCode(500, new { status = "error", message = "Lỗi lấy danh sách cây trồng" });
            }
        }

        /// <summary>
        /// Save Harvest Log
        /// </summary>
        [HttpPost("save-harvest")]
        public async Task<IActionResult> SaveHarvest([FromBody] SaveHarvestRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _gameDataService.SaveHarvestLogAsync(request, cancellationToken);
                
                if (result == "Success")
                {
                    return Ok(new { 
                        status = "success", 
                        message = "Harvest saved successfully",
                        data = new { savedCount = request.Harvests.Count }
                    });
                }
                
                return BadRequest(new { status = "error", message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving harvest for user {UserId}", request.UserId);
                return StatusCode(500, new { status = "error", message = "Lỗi lưu lịch sử thu hoạch" });
            }
        }

        /// <summary>
        /// Get Harvest Log
        /// </summary>
        [HttpGet("harvest-log/{userId}")]
        public async Task<IActionResult> GetHarvestLog(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var harvests = await _gameDataService.GetHarvestLogAsync(userId, cancellationToken);
                return Ok(new { 
                    status = "success", 
                    data = new { userId, harvests }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting harvest log for user {UserId}", userId);
                return StatusCode(500, new { status = "error", message = "Lỗi lấy lịch sử thu hoạch" });
            }
        }

        /// <summary>
        /// Save Inventory
        /// </summary>
        [HttpPost("save-inventory")]
        public async Task<IActionResult> SaveInventory([FromBody] SaveInventoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _gameDataService.SaveInventoryAsync(request, cancellationToken);
                
                if (result == "Success")
                {
                    return Ok(new { 
                        status = "success", 
                        message = "Inventory saved successfully",
                        data = new { savedCount = request.Items.Count }
                    });
                }
                
                return BadRequest(new { status = "error", message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving inventory for user {UserId}", request.UserId);
                return StatusCode(500, new { status = "error", message = "Lỗi lưu kho đồ" });
            }
        }

        /// <summary>
        /// Get Inventory
        /// </summary>
        [HttpGet("inventory/{userId}")]
        public async Task<IActionResult> GetInventory(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var items = await _gameDataService.GetInventoryAsync(userId, cancellationToken);
                return Ok(new { 
                    status = "success", 
                    data = new { userId, items }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inventory for user {UserId}", userId);
                return StatusCode(500, new { status = "error", message = "Lỗi lấy kho đồ" });
            }
        }

        /// <summary>
        /// Update User Stats
        /// </summary>
        [HttpPut("update-stats")]
        public async Task<IActionResult> UpdateUserStats([FromBody] UpdateUserStatsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _gameDataService.UpdateUserStatsAsync(request, cancellationToken);
                
                if (result == "Success")
                {
                    return Ok(new { 
                        status = "success", 
                        message = "Stats updated successfully",
                        data = new { 
                            userId = request.UserId,
                            coin = request.Coin,
                            expPerLevel = request.ExpPerLevel,
                            level = request.Level
                        }
                    });
                }
                
                return BadRequest(new { status = "error", message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stats for user {UserId}", request.UserId);
                return StatusCode(500, new { status = "error", message = "Lỗi cập nhật thống kê" });
            }
        }

        /// <summary>
        /// Get User Stats
        /// </summary>
        [HttpGet("stats/{userId}")]
        public async Task<IActionResult> GetUserStats(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var stats = await _gameDataService.GetUserStatsAsync(userId, cancellationToken);
                
                if (stats == null)
                {
                    return NotFound(new { status = "error", message = "User not found" });
                }

                return Ok(new { 
                    status = "success", 
                    data = stats
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stats for user {UserId}", userId);
                return StatusCode(500, new { status = "error", message = "Lỗi lấy thống kê" });
            }
        }

        /// <summary>
        /// Save Scene Data
        /// </summary>
        [HttpPost("save-scene")]
        public async Task<IActionResult> SaveScene([FromBody] SaveSceneRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _gameDataService.SaveSceneDataAsync(request, cancellationToken);
                
                if (result == "Success")
                {
                    return Ok(new { 
                        status = "success", 
                        message = "Scene saved successfully"
                    });
                }
                
                return BadRequest(new { status = "error", message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving scene for user {UserId}", request.UserId);
                return StatusCode(500, new { status = "error", message = "Lỗi lưu scene" });
            }
        }

        /// <summary>
        /// Get Scene Data
        /// </summary>
        [HttpGet("scene/{userId}")]
        public async Task<IActionResult> GetScene(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var sceneData = await _gameDataService.GetSceneDataAsync(userId, cancellationToken);
                
                if (sceneData == null)
                {
                    return NotFound(new { status = "error", message = "Scene not found" });
                }

                return Ok(new { 
                    status = "success", 
                    data = new { userId, sceneData }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting scene for user {UserId}", userId);
                return StatusCode(500, new { status = "error", message = "Lỗi lấy scene" });
            }
        }

        /// <summary>
        /// Save Scene Details
        /// </summary>
        [HttpPost("save-scene-details")]
        public async Task<IActionResult> SaveSceneDetails([FromBody] SaveSceneDetailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _gameDataService.SaveSceneDetailsAsync(request, cancellationToken);
                
                if (result == "Success")
                {
                    return Ok(new { 
                        status = "success", 
                        message = "Scene details saved successfully"
                    });
                }
                
                return BadRequest(new { status = "error", message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving scene details for user {UserId}", request.UserId);
                return StatusCode(500, new { status = "error", message = "Lỗi lưu chi tiết scene" });
            }
        }

        /// <summary>
        /// Get Scene Details
        /// </summary>
        [HttpGet("scene-details/{userId}")]
        public async Task<IActionResult> GetSceneDetails(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var sceneDetails = await _gameDataService.GetSceneDetailsAsync(userId, cancellationToken);
                return Ok(new { 
                    status = "success", 
                    data = new { userId, sceneDetails }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting scene details for user {UserId}", userId);
                return StatusCode(500, new { status = "error", message = "Lỗi lấy chi tiết scene" });
            }
        }

        /// <summary>
        /// Full Game Data Sync
        /// </summary>
        [HttpPost("sync")]
        public async Task<IActionResult> SyncFullGameData([FromBody] FullGameDataRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _gameDataService.SyncFullGameDataAsync(request, cancellationToken);
                
                return Ok(new { 
                    status = result.Status,
                    message = result.Message,
                    data = new { 
                        syncTimestamp = result.SyncTimestamp,
                        syncedSections = result.SyncedSections
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing full game data for user {UserId}", request.UserId);
                return StatusCode(500, new { status = "error", message = "Lỗi đồng bộ dữ liệu game" });
            }
        }

        /// <summary>
        /// Get Full Game Data
        /// </summary>
        [HttpGet("full-data/{userId}")]
        public async Task<IActionResult> GetFullGameData(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var gameData = await _gameDataService.GetFullGameDataAsync(userId, cancellationToken);
                return Ok(new { 
                    status = "success", 
                    data = gameData
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting full game data for user {UserId}", userId);
                return StatusCode(500, new { status = "error", message = "Lỗi lấy dữ liệu game đầy đủ" });
            }
        }
    }
}
