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
                // Lấy toàn bộ item hiện có của user
                var existing = await _userservicesContext.Categorydetails
                    .Where(c => c.UserId == userId)
                    .ToListAsync(cancellationToken);

                // Dùng HashSet để tìm nhanh itemId từ client
                var clientItemIds = cateDAO.Select(c => c.itemId).ToHashSet();

                // 1️⃣ Xóa những item không còn xuất hiện trong list client
                var toRemove = existing
                    .Where(dbItem => !clientItemIds.Contains(dbItem.ItemId))
                    .ToList();

                if (toRemove.Any())
                {
                    _userservicesContext.Categorydetails.RemoveRange(toRemove);
                    Console.WriteLine($"🗑️ Removed {toRemove.Count} items not present in client data.");
                }

                // 2️⃣ Cập nhật hoặc thêm mới
                foreach (var cate in cateDAO)
                {
                    var existingItem = existing.FirstOrDefault(e => e.ItemId == cate.itemId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity = cate.quantity;
                        _userservicesContext.Categorydetails.Update(existingItem);
                    }
                    else
                    {
                        await _userservicesContext.Categorydetails.AddAsync(new Categorydetail
                        {
                            UserId = userId,
                            ItemId = cate.itemId,
                            Quantity = cate.quantity
                        }, cancellationToken);
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
