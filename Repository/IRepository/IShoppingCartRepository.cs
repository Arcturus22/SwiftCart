using SwiftCart.Data;

namespace SwiftCart.Repository.IRepository
{
    public interface IShoppingCartRepository
    {
        public Task<bool> UpdateCartAsync(string userId, int productId, int updateByQty);
        public Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId);
        public Task<bool> ClearCartAsync(string? userId);

        public Task<int> GetTotalCartCountAsync(string? userId); 

    }
}
