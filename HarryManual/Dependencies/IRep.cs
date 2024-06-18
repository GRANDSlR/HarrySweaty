using System.Collections.Generic;
using System.Windows.Documents;

namespace HarryManual.Dependencies
{
    internal interface IRep<T>
    {
        int AddItem(T item);
        List<T> GetItems();
        int UpdateItem(T item);
        int DeleteItem(int itemId);
    }
}
