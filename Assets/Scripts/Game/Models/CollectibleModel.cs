using UnityEngine;

namespace Game.Models
{
    public record CollectibleModel(int Id, int Type) : IMovingObjectModel
    {
        public Vector3 Position;

        public CollectibleModel(int id, int type, Vector3 position) : this(id, type)
        {
            Position = position;
        }

        public float PositionZ
        {
            get => Position.z;
            set => Position.z = value;
        }
    }
}
