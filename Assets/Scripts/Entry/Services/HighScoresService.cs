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

        private List<Score> hiScores = new();

        public bool IsScoreHighEnough(int score)
        {
            LoadIfShould();

            if (score <= 0)
                return false;

            if (hiScores.Count < maxScores)
                return true;

            return score > hiScores[^1].value;
        }

        public void AddHighScore(Score score)
        {
            LoadIfShould();

            hiScores.Add(score);

            hiScores.Sort((a, b) =>
                          {
                              int compareValues = b.value.CompareTo(a.value);

                              return compareValues != 0 ? compareValues : a.timeTicks.CompareTo(b.timeTicks);
                          });

            hiScores = hiScores.GetRange(0, Mathf.Min(maxScores, hiScores.Count));

            fileService.Save(fileName, hiScores);
        }

        public IEnumerable<Score> GetHighScores()
        {
            LoadIfShould();

            return hiScores.AsReadOnly();
        }

        private void LoadIfShould()
        {
            if (hiScores.Count == 0)
                hiScores = fileService.Load<Score>(fileName).ToList();
        }
    }
}
