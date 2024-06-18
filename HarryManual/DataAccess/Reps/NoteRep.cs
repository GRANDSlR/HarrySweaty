using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class NoteRep : IRep<Notes>
    {
        private DataBaseContext _dbContext;

        public NoteRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddItem(Notes item)
        {
            _dbContext.Notes.Add(item);
            _dbContext.SaveChanges();

            return item.NoteId;
        }

        public int DeleteItem(int itemId)
        {
            var articles = _dbContext.Notes.FirstOrDefault(a => a.NoteId == itemId);

            _dbContext.Notes.Remove(articles);
            _dbContext.SaveChanges();

            return articles.NoteId;
        }

        public List<Notes> GetItems()
        {
            return _dbContext.Notes
                .AsNoTracking()
                .ToList();
        }

        public int UpdateItem(Notes item)
        {
            var objectToUpdate = _dbContext.Notes
                .FirstOrDefault(b => b.NoteId == item.NoteId);

            if(objectToUpdate != null)
            {
                objectToUpdate.NoteTitle = item.NoteTitle;
                objectToUpdate.NoteContent = item.NoteContent;

                _dbContext.SaveChanges();
            }

            return objectToUpdate.NoteId;
        }
    }
}
