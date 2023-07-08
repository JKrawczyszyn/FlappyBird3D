using System;

namespace Entry.Models
{
    [Serializable]
    public struct Asset
    {
        public AssetTag tag;
        public SceneName[] sceneTags;
        public string name;
    }
}
