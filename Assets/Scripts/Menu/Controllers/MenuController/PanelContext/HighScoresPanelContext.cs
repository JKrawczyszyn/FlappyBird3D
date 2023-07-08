using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Entry.Models;

namespace Menu.Controllers
{
    public class HighScoresPanelContext : IPanelContext
    {
        public readonly string Title;
        public readonly IEnumerable<Score> HighScores;
        public readonly string ButtonLabel;
        public bool ButtonPressed { private get; set; }

        public HighScoresPanelContext(string title, IEnumerable<Score> highScores, string buttonLabel)
        {
            Title = title;
            HighScores = highScores;
            ButtonLabel = buttonLabel;
            ButtonPressed = false;
        }

        public async UniTask WaitForButton()
        {
            await UniTask.WaitUntil(() => ButtonPressed);
        }
    }
}
