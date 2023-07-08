namespace Game.Models
{
    public record ObstacleModel(int Id, int Type) : IMovingObjectModel
    {
        public float Position;
        public bool IsPassed;

        public ObstacleModel(int id, int type, float position) : this(id, type)
        {
            Position = position;
            IsPassed = false;
        }

        public float PositionZ
        {
            get => Position;
            set => Position = value;
        }
    }
}
