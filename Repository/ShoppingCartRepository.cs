using Microsoft.EntityFrameworkCore;
using SwiftCart.Data;
using SwiftCart.Repository.IRepository;

namespace SwiftCart.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateByQty)
        {
            if (string.IsNullOrEmpty(userId) || productId <= 0)
            {
                return false; // Invalid parameters
            }

            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = updateByQty
                };
                await _db.ShoppingCart.AddAsync(cart);
            }
            else
            {
                cart.Count += updateByQty;
                if (cart.Count <= 0)
                {
                    _db.ShoppingCart.Remove(cart);
                }
            }
            return await _db.SaveChangesAsync() > 0; // Returns true if any changes were made

        }
        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return await _db.ShoppingCart.ToListAsync();
            }
            return await _db.ShoppingCart.Where(cart => cart.UserId == userId).Include(u => u.Product).ToListAsync();
        }

        public async Task<bool> ClearCartAsync(string? userId)
        {
            var cartItems = await _db.ShoppingCart
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return false; // No items to clear
            }
            _db.ShoppingCart.RemoveRange(cartItems);
            return await _db.SaveChangesAsync() > 0; // Returns true if any changes were made
        }

        public async Task<int> GetTotalCartCountAsync(string? userId)
        {
            int cartCount = 0;  
            var cartItems = await _db.ShoppingCart.Where(cart => cart.UserId == userId).ToListAsync();

            foreach(var item in cartItems)
            {
                cartCount += item.Count;
            }
            return cartCount;
        }
    }
}
