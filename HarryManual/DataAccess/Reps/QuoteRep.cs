using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class QuoteRep : IRep<Quote>
    {
        private DataBaseContext _dbContext;

        public QuoteRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddItem(Quote item)
        {
            _dbContext.Quotes.Add(item);
            _dbContext.SaveChanges();

            return item.QuoteId;
        }

        public int DeleteItem(int itemId)
        {
            var articles = _dbContext.Quotes.FirstOrDefault(a => a.QuoteId == itemId);

            _dbContext.Quotes.Remove(articles);
            _dbContext.SaveChanges();

            return articles.QuoteId;

        }

        public List<Quote> GetItems()
        {
            return _dbContext.Quotes
                .AsNoTracking()
                .ToList();
        }

        public int UpdateItem(Quote item)
        {
            var objectToUpdate = _dbContext.Quotes
                .FirstOrDefault(b => b.QuoteId == item.QuoteId);

            if(objectToUpdate != null)
            {
                objectToUpdate.Content = item.Content;

                _dbContext.SaveChanges();
            }

            return objectToUpdate.QuoteId;
        }
    }
}
