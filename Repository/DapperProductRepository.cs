using Dapper;
using SwiftCart.Data;
using SwiftCart.Repository.IRepository;
using System.Data;

namespace SwiftCart.Repository
{
    public class DapperProductRepository : IProductRepository
    {
        private readonly IDbConnection db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DapperProductRepository(IDbConnection db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<Product> CreateAsync(Product obj)
        {
            var sql = @"INSERT INTO 
                      Product (Name, Description, Price, CategoryId, ImageUrl, SpecialTag)
                      VALUES (@Name, @Description, @Price, @CategoryId, @ImageUrl, @SpecialTag); 
                      SELECT CAST(SCOPE_IDENTITY() as int); ";

            var id = await db.ExecuteScalarAsync<int>(sql, obj);
            obj.Id = id;

            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var getImgUrl = @" SELECT ImageUrl FROM Product WHERE Id = @Id;";
            var imageUrl = await db.QueryFirstOrDefaultAsync<string>
                (
                    getImgUrl, 
                    new { Id = id }
                );

            if(!string.IsNullOrEmpty(imageUrl))
            {
                var imagePath = Path.Combine(webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            var delSql = @" DELETE FROM Product WHERE Id = @Id;";
            var rowsAffected = await db.ExecuteAsync(delSql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // Can't use this as this will not populate the Category Navigation Property we have
            //var GetAllSql = @" SELECT * FROM Product;"; 

            var GetAllSql = @" SELECT p.Id, p.Name, p.price, p.Description, p.SpecialTag, p.CategoryId, p.ImageUrl, c.Id AS CategoryIdSplit, c.Name
                               FROM Product p
                               LEFT JOIN Category c
                               ON p.CategoryId = c.Id;";

            var productDict = new Dictionary<int, Product>();

            var result = await db.QueryAsync<Product, Category, Product>(
                GetAllSql,
                (p, c) =>
                {
                    if (!productDict.TryGetValue(p.Id, out var prod))
                    {
                        prod = p;
                        prod.Category = c;
                        productDict.Add(p.Id, prod);
                    }
                    else
                    {
                        prod.Category = c;
                    }

                    return prod;
                },
                splitOn: "CategoryIdSplit"
                );

            //return result;
            return productDict.Values;

        }

        public async Task<Product> GetAsync(int id)
        {
            var GetProductSql = @"
                                  SELECT 
                                  p.Id, p.Name, p.Price, p.Description, p.SpecialTag, p.CategoryId, p.ImageUrl, c.Id AS CategoryIdSplit, c.Name
                                  FROM Product p
                                  LEFT JOIN Category c
                                  ON p.CategoryId = c.Id
                                  WHERE p.Id = @Id;";

            Product? product = null;

            await db.QueryAsync<Product, Category, Product>(
                GetProductSql,
                (p, c) =>
                {
                    product = p;
                    product.Category = c;
                    return product;
                },
                new { Id = id },
                splitOn: "CategoryIdSplit");

            return product;

        }


        public async Task<Product> UpdateAsync(Product obj)
        {

            var existingProdSql = @"SELECT * FROM Product WHERE Id = @Id;";
            var existingProduct = await db.QueryFirstOrDefaultAsync<Product>(
                existingProdSql,
                obj);

            if(existingProduct is null)
            {
                return obj;
            }

            var updateSql =
                @" UPDATE Product
                SET Name = @Name, Description = @Description, Price = @Price, CategoryId = @CategoryId, ImageUrl = @ImageUrl, SpecialTag = @SpecialTag
                WHERE Id = @Id;";

            await db.ExecuteAsync(updateSql, obj);
            return obj;

        }

    }
}
