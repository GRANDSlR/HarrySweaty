using System.Collections.Generic;
using System.Windows.Documents;

namespace HarryManual.Dependencies
{
    internal interface IRepExtendTitle<T> : IRep<T>
    {
        List<T> GetItems(string title);
    }
}
