namespace Utilities
{
    public static class TypeExtensions
    {
        public static int GetRandom(this int count) => UnityEngine.Random.Range(0, count);
    }
}