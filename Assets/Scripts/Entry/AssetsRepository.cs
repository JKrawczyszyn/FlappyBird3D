using System.Linq;
using Entry.Models;
using UnityEngine;

namespace Entry
{
    [CreateAssetMenu(fileName = "AssetsRepository", menuName = "ScriptableObjects/AssetsRepository", order = 1)]
    public class AssetsRepository : ScriptableObject
    {
        public Asset[] assets;

        public string[] AssetNames(AssetTag assetTag) => assets.FilterWithTag(assetTag).Select(a => a.name).ToArray();

        public int AssetCount(AssetTag assetTag) => assets.Count(a => a.tag == assetTag);

        public string[] AssetNamesForScene(SceneName name) =>
            assets.Where(a => a.sceneTags.Contains(name)).Select(a => a.name).ToArray();

        public Asset[] AssetsForScene(SceneName menu) => assets.Where(a => a.sceneTags.Contains(menu)).ToArray();
    }
}
