using DAL.Context;
using DAL.DAO;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CategoryDetailsRepository
    {
        private readonly db_userservicesContext _userservicesContext;

        public CategoryDetailsRepository(db_userservicesContext db_UserservicesContext)
        {
            _userservicesContext = db_UserservicesContext;
        }

        public async Task<List<Categorydetail>> GetCategorydetailByUserIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                return await _userservicesContext.Categorydetails
                    .AsNoTracking()
                    .Where(c => c.UserId == id)
                    .ToListAsync(cancellationToken);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[CategoryDetailsRepository] GetCategorydetailByUserIdAsync took {sw.ElapsedMilliseconds}ms");
            }
        }

        public async Task<string> SaveCategoryAsync(string userId, List<CateDAO> cateDAO, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                // Build a dictionary of existing items for the user
                var existing = await _userservicesContext.Categorydetails
                    .Where(c => c.UserId == userId)
                    .ToDictionaryAsync(c => c.ItemId, c => c, cancellationToken);

                var upserts = new List<Categorydetail>(cateDAO.Count);

                foreach (var cate in cateDAO)
                {
                    if (existing.TryGetValue(cate.itemId, out var current))
                    {
                        current.Quantity = cate.quantity;
                        upserts.Add(current);
                    }
                    else
                    {
                        upserts.Add(new Categorydetail
                        {
                            UserId = userId,
                            ItemId = cate.itemId,
                            Quantity = cate.quantity
                        });
                    }
                }

                // Use UpdateRange/AddRange with state detection
                foreach (var entry in upserts)
                {
                    var tracked = _userservicesContext.Categorydetails.Local.FirstOrDefault(e => e.UserId == entry.UserId && e.ItemId == entry.ItemId);
                    if (tracked == null)
                    {
                        // Attach as Added or Modified based on existence
                        if (existing.ContainsKey(entry.ItemId))
                        {
                            _userservicesContext.Categorydetails.Update(entry);
                        }
                        else
                        {
                            await _userservicesContext.Categorydetails.AddAsync(entry, cancellationToken);
                        }
                    }
                }

                await _userservicesContext.SaveChangesAsync(cancellationToken);
                return "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[CategoryDetailsRepository] SaveCategoryAsync failed: {e.Message}");
                return e.Message;
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[CategoryDetailsRepository] SaveCategoryAsync took {sw.ElapsedMilliseconds}ms (items={cateDAO?.Count ?? 0})");
            }
        }
    }
}
