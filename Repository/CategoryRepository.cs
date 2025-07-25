﻿using Microsoft.EntityFrameworkCore;
using SwiftCart.Data;
using SwiftCart.Repository.IRepository;

namespace SwiftCart.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category> CreateAsync(Category obj)
        {
            await _db.Category.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
        public async Task<Category> UpdateAsync(Category obj)
        {

            var objFromDb = await _db.Category.FirstOrDefaultAsync(c => c.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                _db.Category.Update(objFromDb);
                await _db.SaveChangesAsync();
                return objFromDb;
            }
            return obj;
        }
        public async Task<bool> DeleteAsync(int id)
        {
         
            var obj = await _db.Category.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null){
                _db.Category.Remove(obj);
                return (await _db.SaveChangesAsync()) > 0;
            }
            return false;
        }
        public async Task<Category> GetAsync(int id)
        {
            var obj = await _db.Category.FirstOrDefaultAsync(c => c.Id == id);
            if(obj == null)
            {
                return new Category(); // Return a new instance if not found
            }
            return obj;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
           return await _db.Category.ToListAsync(); 
        }


    }
}
