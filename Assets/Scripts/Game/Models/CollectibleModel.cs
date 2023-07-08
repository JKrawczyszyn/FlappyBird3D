using UnityEngine;

namespace Game.Models
{
    public record CollectibleModel(int Id) : IMovingObjectModel
    {
        public Vector3 Position;

        public CollectibleModel(int id, Vector3 position) : this(id)
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
