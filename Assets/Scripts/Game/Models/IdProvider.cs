namespace Game.Models
{
    public static class IdProvider
    {
        private static int currentId;

        public static int GetNextId() => ++currentId;
    }
}
