using System.Collections.Generic;
using System.Linq;
using Entry.Models;

namespace Entry.Models
{
    public static class AssetsExtensions
    {
        public static IEnumerable<Asset> FilterWithTag(this IEnumerable<Asset> assets, AssetTag tag) =>
            assets.Where(a => a.assetTag == tag);
    }
}
