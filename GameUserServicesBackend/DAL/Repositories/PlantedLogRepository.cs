using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PlantedLogRepository
    {
        private readonly db_userservicesContext _context;

        public PlantedLogRepository(db_userservicesContext context)
        {
            _context = context;
        }

        public async Task<List<Plantedlog>> GetPlantedLogsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                return await _context.Plantedlogs
                    .AsNoTracking()
                    .Where(p => p.UserId == userId)
                    .ToListAsync(cancellationToken);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[PlantedLogRepository] GetPlantedLogsByUserIdAsync took {sw.ElapsedMilliseconds}ms for UserId: {userId}");
            }
        }

        public async Task<string> SavePlantedLogsAsync(string userId, List<Plantedlog> plantedLogs, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                // Remove existing logs for this user
                var existingLogs = await _context.Plantedlogs
                    .Where(p => p.UserId == userId)
                    .ToListAsync(cancellationToken);

                if (existingLogs.Any())
                {
                    _context.Plantedlogs.RemoveRange(existingLogs);
                }

                // Add new logs
                await _context.Plantedlogs.AddRangeAsync(plantedLogs, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[PlantedLogRepository] SavePlantedLogsAsync failed: {e.Message}");
                return e.Message;
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[PlantedLogRepository] SavePlantedLogsAsync took {sw.ElapsedMilliseconds}ms for {plantedLogs.Count} logs");
            }
        }
    }
}
