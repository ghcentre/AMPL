﻿using System.Collections.Generic;
using System.Web.Optimization;

namespace Ampl.Web.Optimization
{
    internal class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
