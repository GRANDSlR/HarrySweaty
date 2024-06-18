using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class CustomCategory_NoteRep : IRep<CustomCategory_Note>
    {
        private DataBaseContext _dbContext;

        public CustomCategory_NoteRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddItem(CustomCategory_Note item)
        {
            _dbContext.CustomCategory_Notes.Add(item);
            _dbContext.SaveChanges();

            return item.CustomCategory_NoteId;
        }

        public int DeleteItem(int itemId)
        {
            var articles = _dbContext.CustomCategory_Notes.FirstOrDefault(a => a.CustomCategory_NoteId == itemId);

            _dbContext.CustomCategory_Notes.Remove(articles);
            _dbContext.SaveChanges();

            return articles.CustomCategory_NoteId;
        }

        public List<CustomCategory_Note> GetItems()
        {
            return _dbContext.CustomCategory_Notes
                .AsNoTracking()
                .ToList();
        }

        public int UpdateItem(CustomCategory_Note item)
        {
            var objectToUpdate = _dbContext.CustomCategory_Notes
                .FirstOrDefault(b => b.CustomCategory_NoteId == item.CustomCategory_NoteId);

            if(objectToUpdate != null)
            {
                objectToUpdate.CustomCategoryId = item.CustomCategoryId;
                objectToUpdate.NoteId = item.NoteId;

                _dbContext.SaveChanges();
            }

            return objectToUpdate.CustomCategory_NoteId;
        }
    }
}
