using Shunmai.Bxb.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public static class PathHelper
    {
        public static string MapPath(string relativePath)
        {
            Check.Empty(relativePath, nameof(relativePath));

            var seperators = new[] { '\\', '/' };
            var dir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(seperators);
            return $"{dir}/{relativePath.TrimStart(seperators)}";
        }
    }
}
