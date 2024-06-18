using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class CustomCategoryRep : IRep<CustomCategory>
    {
        private DataBaseContext _dbContext;

        public CustomCategoryRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddItem(CustomCategory item)
        {
            _dbContext.CustomCategories.Add(item);
            _dbContext.SaveChanges();
        }

        public void DeleteItem(int itemId)
        {
            var articles = _dbContext.CustomCategories.FirstOrDefault(a => a.CategoryId == itemId);

            _dbContext.CustomCategories.Remove(articles);
            _dbContext.SaveChanges();
        }

        public List<CustomCategory> GetItems()
        {
            return _dbContext.CustomCategories
                .AsNoTracking()
                .ToList();
        }

        public void UpdateItem(CustomCategory item)
        {
            var objectToUpdate = _dbContext.CustomCategories
                .FirstOrDefault(b => b.CategoryId == item.CategoryId);

            if(objectToUpdate != null)
            {
                objectToUpdate.CategoryName = item.CategoryName;
                
                _dbContext.SaveChanges();
            }
        }
    }
}
