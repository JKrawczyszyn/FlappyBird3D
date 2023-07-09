namespace Game.Models
{
    public static class IdProvider
    {
        private static int currentId;

        public static int GetNextId() => ++currentId;

        public static void Reset()
        {
            currentId = 0;
        }
    }
}
