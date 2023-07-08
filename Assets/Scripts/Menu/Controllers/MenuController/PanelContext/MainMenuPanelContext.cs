using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace Menu.Controllers
{
    public class MainMenuPanelContext : IPanelContext
    {
        public readonly string Title;
        public readonly string[] ButtonLabels;
        public int? ButtonPressed { private get; set; }

        public MainMenuPanelContext(string title, params string[] buttonLabels)
        {
            Title = title;
            ButtonLabels = buttonLabels;
            ButtonPressed = null;
        }

        public async UniTask<int> WaitForButton()
        {
            await UniTask.WaitUntil(() => ButtonPressed.HasValue);

            Assert.IsTrue(ButtonPressed.HasValue, $"{nameof(ButtonPressed)} != null");

            return ButtonPressed.Value;
        }
    }
}