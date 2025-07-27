using Microsoft.EntityFrameworkCore;
using SwiftCart.Data;
using SwiftCart.Repository.IRepository;

namespace SwiftCart.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductRepository(ApplicationDbContext db , IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Product> CreateAsync(Product obj)
        {
            await _db.Product.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
        public async Task<Product> UpdateAsync(Product obj)
        {

            var objFromDb = await _db.Product.FirstOrDefaultAsync(c => c.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.ImageUrl = obj.ImageUrl;
                _db.Product.Update(objFromDb);
                await _db.SaveChangesAsync();
                return objFromDb;
            }
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
         
            var obj = await _db.Product.FirstOrDefaultAsync(c => c.Id == id);
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));
            if(File.Exists(imagePath))
            {
                File.Delete(imagePath); // Delete the image file if it exists
            }
            if (obj != null){
                _db.Product.Remove(obj);
                return (await _db.SaveChangesAsync()) > 0;
            }
            return false;
        }

        public async Task<Product> GetAsync(int id)
        {
            var obj = await _db.Product.FirstOrDefaultAsync(c => c.Id == id);
            if(obj == null)
            {
                return new Product(); // Return a new instance if not found
            }
            return obj;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
           return await _db.Product
                                    .Include(p=>p.Category) // Include the Category navigation property
                                    .ToListAsync(); 
        }


    }
}
