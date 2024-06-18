using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class PersonRep : IRepExtend<Person>
    {
        private DataBaseContext _dbContext;

        public PersonRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddItem(Person item)
        {
            _dbContext.Persons.Add(item);
            _dbContext.SaveChanges();

            return item.PersonId;
        }

        public int DeleteItem(int itemId)
        {
            var articles = _dbContext.Persons.FirstOrDefault(a => a.PersonId == itemId);

            _dbContext.Persons.Remove(articles);
            _dbContext.SaveChanges();

            return articles.PersonId;
        }

        public List<Person> GetItems()
        {
            return _dbContext.Persons
                .AsNoTracking()
                .ToList();
        }

        public List<Person> GetItems(string title)
        {
            return _dbContext.Persons
                .AsNoTracking()
                .Where(a => a.Name.ToLower().Contains(title.ToLower()))
                .ToList();
        }

        public int UpdateItem(Person item)
        {
            var objectToUpdate = _dbContext.Persons
                .FirstOrDefault(b => b.PersonId == item.PersonId);

            if(objectToUpdate != null)
            {
                objectToUpdate.Name = item.Name;
                objectToUpdate.Sex = item.Sex;
                objectToUpdate.Description = item.Description;

                _dbContext.SaveChanges();
            }

            return objectToUpdate.PersonId;
        }
    }
}
