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

        public int AddItem(CustomCategory item)
        {
            _dbContext.CustomCategories.Add(item);
            _dbContext.SaveChanges();

            return item.CategoryId;
        }

        public int DeleteItem(int itemId)
        {
            var articles = _dbContext.CustomCategories.FirstOrDefault(a => a.CategoryId == itemId);

            _dbContext.CustomCategories.Remove(articles);
            _dbContext.SaveChanges();

            return articles.CategoryId;
        }

        public List<CustomCategory> GetItems()
        {
            return _dbContext.CustomCategories
                .AsNoTracking()
                .ToList();
        }

        public int UpdateItem(CustomCategory item)
        {
            var objectToUpdate = _dbContext.CustomCategories
                .FirstOrDefault(b => b.CategoryId == item.CategoryId);

            if(objectToUpdate != null)
            {
                objectToUpdate.CategoryName = item.CategoryName;
                
                _dbContext.SaveChanges();
            }

            return objectToUpdate.CategoryId;
        }
    }
}
