namespace Game.Controllers
{
    public static class IdProvider
    {
        private static int currentId;

        public static int GetNextId() => ++currentId;

        public static void ResetId()
        {
            currentId = 0;
        }
    }
}