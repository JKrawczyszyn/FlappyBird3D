using System.Collections.Generic;
using System.Linq;
using Entry.Models;
using UnityEngine;
using Zenject;

namespace Entry.Services
{
    public class HighScoresService : IService
    {
        private const string fileName = "HighScores.bin";
        private const int maxScores = 20;

        [Inject]
        private IFileService fileService;

        private List<Score> highScores = new();

        public void AddHighScore(Score score)
        {
            LoadIfShould();

            if (!IsScoreHighEnough(score.value))
                return;

            highScores.Add(score);

            highScores.Sort((a, b) =>
                          {
                              int compareValues = b.value.CompareTo(a.value);

                              return compareValues != 0 ? compareValues : a.timeTicks.CompareTo(b.timeTicks);
                          });

            highScores = highScores.GetRange(0, Mathf.Min(maxScores, highScores.Count));

            fileService.Save(fileName, highScores);
        }

        public bool IsScoreHighEnough(int score)
        {
            LoadIfShould();

            if (score <= 0)
                return false;

            if (highScores.Count < maxScores)
                return true;

            return score > highScores[^1].value;
        }

        public IEnumerable<Score> GetHighScores()
        {
            LoadIfShould();

            return highScores.AsReadOnly();
        }

        private void LoadIfShould()
        {
            if (highScores.Count == 0)
                highScores = fileService.Load<Score>(fileName).ToList();
        }
    }
}
