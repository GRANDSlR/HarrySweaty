using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class FilmRep : IRep<Film>
    {
        private DataBaseContext _dbContext;

        public FilmRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddItem(Film item)
        {
            _dbContext.Films.Add(item);
            _dbContext.SaveChanges();

            return item.FilmId;
        }

        public int DeleteItem(int itemId)
        {
            var articles = _dbContext.Articles.FirstOrDefault(a => a.ArticleId == itemId);

            _dbContext.Articles.Remove(articles);
            _dbContext.SaveChanges();

            return articles.ArticleId;
        }

        public List<Film> GetItems()
        {
            return _dbContext.Films
                .AsNoTracking()
                .ToList();
        }

        public int UpdateItem(Film item)
        {
            var objectToUpdate = _dbContext.Films
                .FirstOrDefault(b => b.FilmId == item.FilmId);

            if(objectToUpdate != null)
            {
                objectToUpdate.Title = item.Title;
                objectToUpdate.Description = item.Description;
                objectToUpdate.DateOfPublication = item.DateOfPublication;
                objectToUpdate.Part = item.Part;

                _dbContext.SaveChanges();
            }

            return objectToUpdate.FilmId;
        }
    }
}
