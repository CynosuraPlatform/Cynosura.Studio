using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class UpgradeMetadata
    {
        public int Version { get; set; }
        public IList<UpgradeItem> Upgrades { get; set; }
    }

    public class UpgradeItem
    {
        public int From { get; set; }
        public int To { get; set; }
        public IList<UpgradeRename> Renames { get; set; }
    }

    public class UpgradeRename
    {
        public string Left { get; set; }
        public string Right { get; set; }
    }
}
