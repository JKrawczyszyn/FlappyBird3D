using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Entry.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace Entry.Services
{
    public class ScoresService : IService
    {
        private const string fileName = "HighScores.bin";
        private const int maxScores = 20;

        private readonly BinaryFormatter binaryFormatter = new();

        private List<Score> hiScores = new();

        private readonly string fullPath = Path.Join(Application.persistentDataPath, fileName);

        public bool IsScoreHighEnough(int score)
        {
            if (hiScores.Count == 0)
                Load();

            if (hiScores.Count < maxScores)
                return true;

            return score > hiScores[^1].value;
        }

        public void AddHighScore(int score, DateTime time, string name)
        {
            if (hiScores.Count == 0)
                Load();

            hiScores.Add(new Score(score, time, name));
            hiScores.Sort((a, b) => b.value.CompareTo(a.value));
            hiScores = hiScores.GetRange(0, Mathf.Min(maxScores, hiScores.Count));

            Save();
        }

        public IEnumerable<Score> GetHiScores()
        {
            if (hiScores.Count == 0)
                Load();

            return hiScores.AsReadOnly();
        }

        private void Load()
        {
            if (!File.Exists(fullPath))
                return;

            using FileStream file = File.Open(fullPath, FileMode.Open);

            hiScores = (List<Score>)binaryFormatter.Deserialize(file);
        }

        private void Save()
        {
            Assert.IsTrue(hiScores.Count > 0, "No scores to save.");

            using FileStream file = File.Create(fullPath);

            binaryFormatter.Serialize(file, hiScores);
        }
    }
}
