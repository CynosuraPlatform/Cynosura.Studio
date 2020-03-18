﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Generator.Merge
{
    public class DmpMerge : IFileMerge
    {
        public string Merge(string original, string their, string my)
        {
            var dmp = new DiffMatchPatch.diff_match_patch();
            var patches = dmp.patch_make(original, their);
            var result = dmp.patch_apply(patches, my);
            return (string)result[0];
        }
    }
}
