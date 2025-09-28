using BLL.Models;
using DAL.Context;
using DAL.DAO;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class GameDataService
    {
        private readonly db_userservicesContext _context;
        private readonly PlantedLogRepository _plantedLogRepository;
        private readonly SceneRepository _sceneRepository;
        private readonly SceneDetailRepository _sceneDetailRepository;
        private readonly CategoryDetailsRepository _categoryDetailsRepository;

        public GameDataService(
            db_userservicesContext context,
            PlantedLogRepository plantedLogRepository,
            SceneRepository sceneRepository,
            SceneDetailRepository sceneDetailRepository,
            CategoryDetailsRepository categoryDetailsRepository)
        {
            _context = context;
            _plantedLogRepository = plantedLogRepository;
            _sceneRepository = sceneRepository;
            _sceneDetailRepository = sceneDetailRepository;
            _categoryDetailsRepository = categoryDetailsRepository;
        }

        // Planted Trees Methods
        public async Task<List<PlantedTreeDto>> GetPlantedTreesAsync(string userId, CancellationToken cancellationToken = default)
        {
            var plantedLogs = await _plantedLogRepository.GetPlantedLogsByUserIdAsync(userId, cancellationToken);
            return plantedLogs.Select(p => new PlantedTreeDto
            {
                ItemId = p.ItemId ?? string.Empty,
                Status = p.Status ?? string.Empty
            }).ToList();
        }

        public async Task<string> SavePlantedTreesAsync(SavePlantedTreesRequest request, CancellationToken cancellationToken = default)
        {
            var plantedLogs = request.Trees.Select(t => new Plantedlog
            {
                UserId = request.UserId,
                ItemId = t.ItemId,
                Status = t.Status
            }).ToList();

            return await _plantedLogRepository.SavePlantedLogsAsync(request.UserId, plantedLogs, cancellationToken);
        }

        // Harvest Log Methods
        public async Task<List<HarvestDto>> GetHarvestLogAsync(string userId, CancellationToken cancellationToken = default)
        {
            var plantedLogs = await _plantedLogRepository.GetPlantedLogsByUserIdAsync(userId, cancellationToken);
            return plantedLogs
                .Where(p => p.Status == "harvested")
                .Select(p => new HarvestDto
                {
                    ItemId = p.ItemId ?? string.Empty,
                    Status = p.Status ?? string.Empty
                }).ToList();
        }

        public async Task<string> SaveHarvestLogAsync(SaveHarvestRequest request, CancellationToken cancellationToken = default)
        {
            var plantedLogs = request.Harvests.Select(h => new Plantedlog
            {
                UserId = request.UserId,
                ItemId = h.ItemId,
                Status = h.Status
            }).ToList();

            return await _plantedLogRepository.SavePlantedLogsAsync(request.UserId, plantedLogs, cancellationToken);
        }

        // Inventory Methods
        public async Task<List<InventoryItemDto>> GetInventoryAsync(string userId, CancellationToken cancellationToken = default)
        {
            var categoryDetails = await _categoryDetailsRepository.GetCategorydetailByUserIdAsync(userId, cancellationToken);
            return categoryDetails.Select(c => new InventoryItemDto
            {
                ItemId = c.ItemId,
                Quantity = c.Quantity
            }).ToList();
        }

        public async Task<string> SaveInventoryAsync(SaveInventoryRequest request, CancellationToken cancellationToken = default)
        {
            var categoryDetails = request.Items.Select(i => new CateDAO
            {
                itemId = i.ItemId,
                quantity = i.Quantity
            }).ToList();

            return await _categoryDetailsRepository.SaveCategoryAsync(request.UserId, categoryDetails, cancellationToken);
        }

        // User Stats Methods
        public async Task<UserStatsDto?> GetUserStatsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

                if (user == null) return null;

                return new UserStatsDto
                {
                    UserId = user.UserId,
                    Coin = user.Coin ?? 0,
                    ExpPerLevel = user.ExpPerLevel ?? 0,
                    Level = user.Level ?? 0,
                    MemberTypeId = user.MemberTypeId,
                    Status = user.Status
                };
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[GameDataService] GetUserStatsAsync took {sw.ElapsedMilliseconds}ms for UserId: {userId}");
            }
        }

        public async Task<string> UpdateUserStatsAsync(UpdateUserStatsRequest request, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

                if (user == null)
                {
                    return "User not found";
                }

                user.Coin = request.Coin;
                user.ExpPerLevel = request.ExpPerLevel;
                user.Level = request.Level;

                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);

                return "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[GameDataService] UpdateUserStatsAsync failed: {e.Message}");
                return e.Message;
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[GameDataService] UpdateUserStatsAsync took {sw.ElapsedMilliseconds}ms for UserId: {request.UserId}");
            }
        }

        // Scene Data Methods
        public async Task<SceneDataDto?> GetSceneDataAsync(string userId, CancellationToken cancellationToken = default)
        {
            var scene = await _sceneRepository.GetSceneByUserIdAsync(userId, cancellationToken);
            if (scene == null) return null;

            return new SceneDataDto
            {
                Status = scene.Status ?? string.Empty,
                DateSave = scene.DateSave ?? DateTime.Now
            };
        }

        public async Task<string> SaveSceneDataAsync(SaveSceneRequest request, CancellationToken cancellationToken = default)
        {
            var scene = new Scene
            {
                UserId = request.UserId,
                Status = request.SceneData.Status,
                DateSave = request.SceneData.DateSave
            };

            return await _sceneRepository.SaveSceneAsync(scene, cancellationToken);
        }

        // Scene Details Methods
        public async Task<List<SceneDetailDto>> GetSceneDetailsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var sceneDetails = await _sceneDetailRepository.GetSceneDetailsByUserIdAsync(userId, cancellationToken);
            return sceneDetails.Select(sd => new SceneDetailDto
            {
                ItemId = sd.ItemId ?? string.Empty,
                Name = sd.Name ?? string.Empty
            }).ToList();
        }

        public async Task<string> SaveSceneDetailsAsync(SaveSceneDetailsRequest request, CancellationToken cancellationToken = default)
        {
            var sceneDetails = request.SceneDetails.Select(sd => new Scenedetail
            {
                UserId = request.UserId,
                ItemId = sd.ItemId,
                Name = sd.Name
            }).ToList();

            return await _sceneDetailRepository.SaveSceneDetailsAsync(request.UserId, sceneDetails, cancellationToken);
        }

        // Full Game Data Sync
        public async Task<FullGameDataResponse> GetFullGameDataAsync(string userId, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                // Execute all tasks in parallel
                var userStatsTask = GetUserStatsAsync(userId, cancellationToken);
                var inventoryTask = GetInventoryAsync(userId, cancellationToken);
                var plantedTreesTask = GetPlantedTreesAsync(userId, cancellationToken);
                var harvestLogTask = GetHarvestLogAsync(userId, cancellationToken);
                var sceneDataTask = GetSceneDataAsync(userId, cancellationToken);

                await Task.WhenAll(userStatsTask, inventoryTask, plantedTreesTask, harvestLogTask, sceneDataTask);

                return new FullGameDataResponse
                {
                    UserId = userId,
                    UserStats = await userStatsTask,
                    Inventory = await inventoryTask,
                    PlantedTrees = await plantedTreesTask,
                    HarvestLog = await harvestLogTask,
                    SceneData = await sceneDataTask
                };
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[GameDataService] GetFullGameDataAsync took {sw.ElapsedMilliseconds}ms for UserId: {userId}");
            }
        }

        public async Task<SyncResponse> SyncFullGameDataAsync(FullGameDataRequest request, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var syncedSections = new List<string>();

            try
            {
                var tasks = new List<Task<string>>();

                if (request.UserStats != null)
                {
                    tasks.Add(UpdateUserStatsAsync(new UpdateUserStatsRequest
                    {
                        UserId = request.UserId,
                        Coin = request.UserStats.Coin,
                        ExpPerLevel = request.UserStats.ExpPerLevel,
                        Level = request.UserStats.Level
                    }, cancellationToken));
                }

                if (request.Inventory != null)
                {
                    tasks.Add(SaveInventoryAsync(new SaveInventoryRequest
                    {
                        UserId = request.UserId,
                        Items = request.Inventory
                    }, cancellationToken));
                }

                if (request.PlantedTrees != null)
                {
                    tasks.Add(SavePlantedTreesAsync(new SavePlantedTreesRequest
                    {
                        UserId = request.UserId,
                        Trees = request.PlantedTrees
                    }, cancellationToken));
                }

                if (request.HarvestLog != null)
                {
                    tasks.Add(SaveHarvestLogAsync(new SaveHarvestRequest
                    {
                        UserId = request.UserId,
                        Harvests = request.HarvestLog
                    }, cancellationToken));
                }

                if (request.SceneData != null)
                {
                    tasks.Add(SaveSceneDataAsync(new SaveSceneRequest
                    {
                        UserId = request.UserId,
                        SceneData = request.SceneData
                    }, cancellationToken));
                }

                var results = await Task.WhenAll(tasks);

                // Check which sections were successfully synced
                var resultIndex = 0;
                if (request.UserStats != null)
                {
                    if (results[resultIndex] == "Success") syncedSections.Add("userStats");
                    resultIndex++;
                }
                if (request.Inventory != null)
                {
                    if (results[resultIndex] == "Success") syncedSections.Add("inventory");
                    resultIndex++;
                }
                if (request.PlantedTrees != null)
                {
                    if (results[resultIndex] == "Success") syncedSections.Add("plantedTrees");
                    resultIndex++;
                }
                if (request.HarvestLog != null)
                {
                    if (results[resultIndex] == "Success") syncedSections.Add("harvestLog");
                    resultIndex++;
                }
                if (request.SceneData != null)
                {
                    if (results[resultIndex] == "Success") syncedSections.Add("sceneData");
                    resultIndex++;
                }

                return new SyncResponse
                {
                    Status = "success",
                    Message = "Game data synchronized successfully",
                    SyncTimestamp = DateTime.Now,
                    SyncedSections = syncedSections
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[GameDataService] SyncFullGameDataAsync failed: {e.Message}");
                return new SyncResponse
                {
                    Status = "error",
                    Message = e.Message,
                    SyncTimestamp = DateTime.Now,
                    SyncedSections = syncedSections
                };
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[GameDataService] SyncFullGameDataAsync took {sw.ElapsedMilliseconds}ms for UserId: {request.UserId}");
            }
        }
    }
}
