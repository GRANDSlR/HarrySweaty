using System.Collections.Generic;
using System.Windows.Documents;

namespace HarryManual.Dependencies
{
    internal interface IRepExtendId<T> : IRep<T>
    {
        List<T> GetItems(int id);
    }
}
