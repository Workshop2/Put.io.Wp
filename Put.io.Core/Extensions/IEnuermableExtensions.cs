using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Put.io.Core.Extensions
{
    public static class IEnumerableExtensions
    {
         public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
         {
             var result = new ObservableCollection<T>();

             foreach (var item in list)
             {
                 result.Add(item);
             }

             return result;
         }
    }
}