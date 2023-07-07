using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class CollectionsExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            T[] array = collection as T[] ?? collection.ToArray();

            return array[UnityEngine.Random.Range(0, array.Length)];
        }
    }
}
