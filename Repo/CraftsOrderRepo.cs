using Microsoft.EntityFrameworkCore;
using PersonalERP.Entity;
using PersonalERP.Interface;
namespace PersonalERP.Repo
{
    public class CraftsOrderRepo: ICraftsOrderRepo
    {
        private readonly AppDbContext _appDbContext;

        public CraftsOrderRepo(AppDbContext appDbContext)
        {
            try
            {
                _appDbContext = appDbContext;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the order: {ex.Message}", ex);
            }
        }

        public async Task<int> CreateAsync(CraftsOrder newOrder)
        {
            try
            {
                _appDbContext.CraftsOrders.Add(newOrder);
                await _appDbContext.SaveChangesAsync();
                return newOrder.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting the order: {ex.Message}", ex);
            }
        }

        public async Task<CraftsOrder?> GetByIdAsync(int id)
        {
            try
            {
                return await _appDbContext.CraftsOrders.Include(x => x.Customer).Include(x => x.Art).FirstOrDefaultAsync(y => y.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting the order: {ex.Message}", ex);
            }
        }

        public async Task<CraftsOrder?> GetInfoByCraftsOrder(int craftsOrderId)
        {
            try
            {
                var craftsOrderInfo = await _appDbContext.CraftsOrders.FindAsync(craftsOrderId);
                if (craftsOrderInfo != null)
                {
                    return craftsOrderInfo;
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public async Task<IEnumerable<CraftsOrder>> GetAllAsync()
        //{
        //    try
        //    {
        //        return await _appDbContext.CraftsOrders
        //                       .Include(c => c.Customer)
        //                       .Include(x => x.Art)
        //                       .ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"An error occurred while getting the order: {ex.Message}", ex);
        //    }
        //}
        //// In CraftsOrderRepository.cs
        public async Task<IEnumerable<CraftsOrder>> GetAllAsync()
        {
            try
            {
                return await _appDbContext.CraftsOrders
                                .Include(c => c.Customer)
                                .Include(x => x.Art)
                                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting the order: {ex.Message}", ex);
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var order = await _appDbContext.CraftsOrders.FindAsync(id);
                if (order == null) return false;

                _appDbContext.CraftsOrders.Remove(order);
                return await _appDbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the order: {ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(CraftsOrder craftsOrder)
        {
            try
            {
                _appDbContext.CraftsOrders.Update(craftsOrder);
                await _appDbContext.SaveChangesAsync();
                //return craftsOrder;
            }
            catch (Exception ex)
            {
                throw new Exception("Problem here");
            }
        }

    }
}
