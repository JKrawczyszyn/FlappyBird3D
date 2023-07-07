using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetsRepository", menuName = "ScriptableObjects/AssetsRepository", order = 1)]
public class AssetsRepository : ScriptableObject
{
    public Asset[] assets;

    public string[] AssetNames(AssetTag assetTag) => assets.Where(a => a.tag == assetTag).Select(a => a.name).ToArray();

    public int AssetCount(AssetTag assetTag) => assets.Count(a => a.tag == assetTag);

    public string[] AssetNames(IEnumerable<AssetTag> tags)
    {
        var hashSet = tags.ToHashSet();

        return assets.Where(a => hashSet.Contains(a.tag)).Select(a => a.name).ToArray();
    }

    public string[] AssetNamesForScene(SceneName name) =>
        assets.Where(a => a.sceneTags.Contains(name)).Select(a => a.name).ToArray();

    public Asset[] AssetsForScene(SceneName menu) => assets.Where(a => a.sceneTags.Contains(menu)).ToArray();
}

[Serializable]
public struct Asset
{
    public AssetTag tag;
    public SceneName[] sceneTags;
    public string name;
}

public enum AssetTag
{
    None,
    Bird,
    Walls,
    Obstacle,
    MenuButton,
    MenuText,
}

public enum SceneName
{
    None,
    Menu,
    Game,
}
