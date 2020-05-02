using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Generator.PackageFeed.Models
{
    class VersionComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var xSplit = x.Split('.', '-');
            var ySplit = y.Split('.', '-');
            for (int i = 0; i < Math.Min(xSplit.Length, ySplit.Length); i++)
            {
                int compare;
                if (int.TryParse(xSplit[i], out var xNumber) && int.TryParse(ySplit[i], out var yNumber))
                {
                    compare = xNumber.CompareTo(yNumber);
                }
                else
                {
                    compare = xSplit[i].CompareTo(ySplit[i]);
                }
                if (compare != 0)
                    return compare;
            }
            return 0;
        }
    }
}