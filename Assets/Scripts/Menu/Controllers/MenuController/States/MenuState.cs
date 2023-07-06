using Utilities;
using Zenject;

namespace Menu.Controllers
{
    public abstract class MenuState : IState
    {
        [Inject]
        protected MenuController MenuController;

        public abstract void Perform();
    }
}
