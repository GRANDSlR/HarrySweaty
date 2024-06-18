using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class Person_QuoteRep : IRep<Person_Quote>
    {
        private DataBaseContext _dbContext;

        public Person_QuoteRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddItem(Person_Quote item)
        {
            _dbContext.Person_Quotes.Add(item);
            _dbContext.SaveChanges();
        }

        public void DeleteItem(int itemId)
        {
            var articles = _dbContext.Person_Quotes.FirstOrDefault(a => a.Person_QuoteId == itemId);

            _dbContext.Person_Quotes.Remove(articles);
            _dbContext.SaveChanges();
        }

        public List<Person_Quote> GetItems()
        {
            return _dbContext.Person_Quotes
                .AsNoTracking()
                .ToList();
        }

        public void UpdateItem(Person_Quote item)
        {
            var objectToUpdate = _dbContext.Person_Quotes
                .FirstOrDefault(b => b.Person_QuoteId == item.Person_QuoteId);

            if(objectToUpdate != null)
            {
                objectToUpdate.PersonId = item.PersonId;
                objectToUpdate.QuoteId = item.QuoteId;

                _dbContext.SaveChanges();
            }
        }
    }
}
