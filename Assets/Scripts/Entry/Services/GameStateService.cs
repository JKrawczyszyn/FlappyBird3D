using Entry.Models;

namespace Entry.Services
{
    // Used to pass data between scenes.
    // Simple, because it has only one property and doesn't need to store data between sessions.
    public class GameStateService : IService
    {
        private Score lastScore;

        public void SetLastScore(Score score)
        {
            lastScore = score;
        }

        public Score GetLastScore()
        {
            var result = lastScore;
            lastScore = null;

            return result;
        }
    }
}
