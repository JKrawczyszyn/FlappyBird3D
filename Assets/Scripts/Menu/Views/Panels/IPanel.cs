using UnityEngine;

namespace Menu.Views
{
    public interface IPanel
    {
        GameObject GameObject { get; }
        void Back();
        void Move();
    }
}
