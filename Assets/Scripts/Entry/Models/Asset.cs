using System;

namespace Entry.Models
{
    [Serializable]
    public struct Asset
    {
        public string name;
        public AssetTag assetTag;
        public SceneName sceneName;
    }
}
