using Cysharp.Threading.Tasks;

namespace Menu.Controllers
{
    public class SetHighScorePanelContext : IPanelContext
    {
        public readonly string Title;
        public string Name;

        public bool NameEntered { private get; set; }

        public SetHighScorePanelContext(string title, string name)
        {
            Title = title;
            Name = name;
        }

        public async UniTask<string> WaitForInput()
        {
            await UniTask.WaitUntil(() => NameEntered);

            return Name;
        }
    }
}
