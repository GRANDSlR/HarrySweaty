using HarryManual.DataAccess.HarryCarrier;
using HarryManual.Dependencies;
using System.Collections.Generic;
using System.Linq;

namespace HarryManual.DataAccess.Reps
{
    public class FavoriteRep : IRepExtendId<Favorite>
    {
        private DataBaseContext _dbContext;

        public FavoriteRep(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int AddItem(Favorite item)
        {
            _dbContext.Favorites.Add(item);
            _dbContext.SaveChanges();

            return item.FavoriteId;
        }

        public int DeleteItem(int itemId)
        {
            var favorite = _dbContext.Favorites.FirstOrDefault(f => f.FavoriteId == itemId);

            _dbContext.Favorites.Remove(favorite);
            _dbContext.SaveChanges();

            return favorite.FavoriteId;
        }

        public List<Favorite> GetItems()
        {
            return _dbContext.Favorites
                .AsNoTracking()
                .ToList();
        }

        public List<Favorite> GetItems(int id)
        {
            return _dbContext.Favorites
                .AsNoTracking()
                .Where(a => a.UserId == id)
                .ToList();
        }

        public int UpdateItem(Favorite item)
        {
            var objectToUpdate = _dbContext.Favorites
                .FirstOrDefault(f => f.FavoriteId == item.FavoriteId);

            if (objectToUpdate != null)
            {
                objectToUpdate.UserId = item.UserId;
                objectToUpdate.NoteId = item.NoteId;
                objectToUpdate.Classification = item.Classification;

                _dbContext.SaveChanges();
            }

            return objectToUpdate.FavoriteId;
        }
    }

}
