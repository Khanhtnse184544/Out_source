using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SceneDetailRepository
    {
        private readonly db_userservicesContext _context;

        public SceneDetailRepository(db_userservicesContext context)
        {
            _context = context;
        }

        public async Task<List<Scenedetail>> GetSceneDetailsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                return await _context.Scenedetails
                    .AsNoTracking()
                    .Where(sd => sd.UserId == userId)
                    .ToListAsync(cancellationToken);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[SceneDetailRepository] GetSceneDetailsByUserIdAsync took {sw.ElapsedMilliseconds}ms for UserId: {userId}");
            }
        }

        public async Task<string> SaveSceneDetailsAsync(string userId, List<Scenedetail> sceneDetails, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                // Remove existing scene details for this user
                var existingDetails = await _context.Scenedetails
                    .Where(sd => sd.UserId == userId)
                    .ToListAsync(cancellationToken);

                if (existingDetails.Any())
                {
                    _context.Scenedetails.RemoveRange(existingDetails);
                }

                // Add new scene details
                await _context.Scenedetails.AddRangeAsync(sceneDetails, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[SceneDetailRepository] SaveSceneDetailsAsync failed: {e.Message}");
                return e.Message;
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[SceneDetailRepository] SaveSceneDetailsAsync took {sw.ElapsedMilliseconds}ms for {sceneDetails.Count} details");
            }
        }
    }
}
