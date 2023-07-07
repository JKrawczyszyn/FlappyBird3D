namespace Game.Controllers
{
    public record ObstacleModel(int Id, int Type)
    {
        public float Position;
        public bool IsPassed;

        public ObstacleModel(int id, int type, float position) : this(id, type)
        {
            Position = position;
        }
    };
}
