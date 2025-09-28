using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SceneRepository
    {
        private readonly db_userservicesContext _context;

        public SceneRepository(db_userservicesContext context)
        {
            _context = context;
        }

        public async Task<Scene?> GetSceneByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                return await _context.Scenes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[SceneRepository] GetSceneByUserIdAsync took {sw.ElapsedMilliseconds}ms for UserId: {userId}");
            }
        }

        public async Task<string> SaveSceneAsync(Scene scene, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var existingScene = await _context.Scenes
                    .FirstOrDefaultAsync(s => s.UserId == scene.UserId, cancellationToken);

                if (existingScene != null)
                {
                    existingScene.Status = scene.Status;
                    existingScene.DateSave = scene.DateSave;
                    _context.Scenes.Update(existingScene);
                }
                else
                {
                    await _context.Scenes.AddAsync(scene, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[SceneRepository] SaveSceneAsync failed: {e.Message}");
                return e.Message;
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[SceneRepository] SaveSceneAsync took {sw.ElapsedMilliseconds}ms for UserId: {scene.UserId}");
            }
        }
    }
}
