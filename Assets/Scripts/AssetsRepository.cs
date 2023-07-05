using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetsRepository", menuName = "ScriptableObjects/AssetsRepository", order = 1)]
public class AssetsRepository : ScriptableObject
{
    public Asset[] assets;

    public string[] AssetNames(AssetTag assetTag) => assets.Where(a => a.tag == assetTag).Select(a => a.name).ToArray();

    public string[] AssetNames(IEnumerable<AssetTag> tags)
    {
        var hashSet = tags.ToHashSet();

        return assets.Where(a => hashSet.Contains(a.tag)).Select(a => a.name).ToArray();
    }
}

[Serializable]
public struct Asset
{
    public AssetTag tag;
    public string name;
}

public enum AssetTag
{
    None,
    Bird,
    Walls,
    Obstacle,
}
