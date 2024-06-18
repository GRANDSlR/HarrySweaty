using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class PersonRep : IRep<Person>
    {
        private DataBaseContext _dbContext;

        public PersonRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddItem(Person item)
        {
            _dbContext.Persons.Add(item);
            _dbContext.SaveChanges();
        }

        public void DeleteItem(int itemId)
        {
            var articles = _dbContext.Persons.FirstOrDefault(a => a.PersonId == itemId);

            _dbContext.Persons.Remove(articles);
            _dbContext.SaveChanges();
        }

        public List<Person> GetItems()
        {
            return _dbContext.Persons
                .AsNoTracking()
                .ToList();
        }

        public void UpdateItem(Person item)
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
        }
    }
}
