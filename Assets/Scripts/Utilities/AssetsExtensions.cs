using System.Collections.Generic;
using System.Linq;
using Entry.Models;

namespace Utilities
{
    public static class AssetsExtensions
    {
        public static IEnumerable<Asset> FilterWithTag(this IEnumerable<Asset> assets, AssetTag tag) =>
            assets.Where(a => a.tag == tag);
    }
}
