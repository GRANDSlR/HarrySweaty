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

        public void AddItem(Notes item)
        {
            _dbContext.Notes.Add(item);
            _dbContext.SaveChanges();
        }

        public void DeleteItem(int itemId)
        {
            var articles = _dbContext.Notes.FirstOrDefault(a => a.NoteId == itemId);

            _dbContext.Notes.Remove(articles);
            _dbContext.SaveChanges();
        }

        public List<Notes> GetItems()
        {
            return _dbContext.Notes
                .AsNoTracking()
                .ToList();
        }

        public void UpdateItem(Notes item)
        {
            var objectToUpdate = _dbContext.Notes
                .FirstOrDefault(b => b.NoteId == item.NoteId);

            if(objectToUpdate != null)
            {
                objectToUpdate.NoteTitle = item.NoteTitle;
                objectToUpdate.NoteContent = item.NoteContent;

                _dbContext.SaveChanges();
            }
        }
    }
}
