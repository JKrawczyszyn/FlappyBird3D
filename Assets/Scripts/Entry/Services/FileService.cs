using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Entry.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace Entry.Services
{
    public interface IFileService
    {
        IEnumerable<Score> Load(string fileName);
        void Save(string fileName, ICollection scores);
    }

    public class FileService : IService, IFileService
    {
        private readonly BinaryFormatter binaryFormatter = new();

        public IEnumerable<Score> Load(string fileName)
        {
            if (!File.Exists(GetFullPath(fileName)))
                return Enumerable.Empty<Score>();

            using FileStream file = File.Open(GetFullPath(fileName), FileMode.Open);

            return (ICollection<Score>)binaryFormatter.Deserialize(file);
        }

        public void Save(string fileName, ICollection scores)
        {
            Assert.IsTrue(scores.Count > 0, "No scores to save.");

            using FileStream file = File.Create(GetFullPath(fileName));

            binaryFormatter.Serialize(file, scores);
        }

        private string GetFullPath(string fileName) => Path.Join(Application.persistentDataPath, fileName);
    }
}
