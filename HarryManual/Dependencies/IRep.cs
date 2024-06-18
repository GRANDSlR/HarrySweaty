using System.Collections.Generic;
using System.Windows.Documents;

namespace HarryManual.Dependencies
{
    internal interface IRep<T>
    {
        void AddItem(T item);
        List<T> GetItems();
        void UpdateItem(T item);
        void DeleteItem(int itemId);
    }
}
