namespace Utilities
{
    public static class CollectionsExtensions
    {
        public static T GetRandom<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
    }
}
