using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.Merge
{
    public interface IMerge
    {
        string Merge(string original, string their, string my);
    }
}
