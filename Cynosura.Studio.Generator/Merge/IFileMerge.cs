﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Generator.Merge
{
    public interface IFileMerge
    {
        string Merge(string original, string their, string my);
    }
}
