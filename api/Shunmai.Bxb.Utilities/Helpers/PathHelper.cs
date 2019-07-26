using System;

namespace Shunmai.Bxb.Utilities.Helpers
{
    public static class PathHelper
    {
        public static string MapPath(string relativePath)
        {
            Check.Check.Empty(relativePath, nameof(relativePath));

            var seperators = new[] { '\\', '/' };
            var dir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(seperators);
            return $"{dir}/{relativePath.TrimStart(seperators)}";
        }
    }
}
