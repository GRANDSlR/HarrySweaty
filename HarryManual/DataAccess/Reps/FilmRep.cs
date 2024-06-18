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

        public void AddItem(Film item)
        {
            _dbContext.Films.Add(item);
            _dbContext.SaveChanges();
        }

        public void DeleteItem(int itemId)
        {
            var articles = _dbContext.Articles.FirstOrDefault(a => a.ArticleId == itemId);

            _dbContext.Articles.Remove(articles);
            _dbContext.SaveChanges();
        }

        public List<Film> GetItems()
        {
            return _dbContext.Films
                .AsNoTracking()
                .ToList();
        }

        public void UpdateItem(Film item)
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
        }
    }
}
