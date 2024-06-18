using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class Person_FilmRep : IRep<Person_Film>
    {
        private DataBaseContext _dbContext;

        public Person_FilmRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddItem(Person_Film item)
        {
            _dbContext.Person_Films.Add(item);
            _dbContext.SaveChanges();

            return item.Person_FilmId;
        }

        public int DeleteItem(int itemId)
        {
            var articles = _dbContext.Person_Films.FirstOrDefault(a => a.Person_FilmId == itemId);

            _dbContext.Person_Films.Remove(articles);
            _dbContext.SaveChanges();

            return articles.Person_FilmId;
        }

        public List<Person_Film> GetItems()
        {
            return _dbContext.Person_Films
                .AsNoTracking()
                .ToList();
        }

        public int UpdateItem(Person_Film item)
        {
            var objectToUpdate = _dbContext.Person_Films
                .FirstOrDefault(b => b.Person_FilmId == item.Person_FilmId);

            if(objectToUpdate != null)
            {
                objectToUpdate.PersonId = item.PersonId;
                objectToUpdate.FilmId = item.FilmId;

                _dbContext.SaveChanges();
            }

            return objectToUpdate.Person_FilmId;
        }
    }
}
