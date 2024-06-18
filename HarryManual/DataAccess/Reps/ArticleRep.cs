using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class ArticleRep : IRep<Article>
    {
        private DataBaseContext _dbContext;

        public ArticleRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddItem(Article item)
        {
            _dbContext.Articles.Add(item);
            _dbContext.SaveChanges();

            return item.ArticleId;
        }

        public int DeleteItem(int itemId)
        {
            var articles = _dbContext.Articles.FirstOrDefault(a => a.ArticleId == itemId);

            _dbContext.Articles.Remove(articles);
            _dbContext.SaveChanges();

            return articles.ArticleId;
        }

        public List<Article> GetItems()
        {
            return _dbContext.Articles
                .AsNoTracking()
                .ToList();
        }

        public int UpdateItem(Article item)
        {
            var objectToUpdate = _dbContext.Articles
                .FirstOrDefault(b => b.ArticleId == item.ArticleId);

            if(objectToUpdate != null)
            {
                objectToUpdate.Title = item.Title;
                objectToUpdate.Description = item.Description;
                objectToUpdate.DateOfPublication = item.DateOfPublication;

                _dbContext.SaveChanges();
            }

            return objectToUpdate.ArticleId;

        }
    }
}
