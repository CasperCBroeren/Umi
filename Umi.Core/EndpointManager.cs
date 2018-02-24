using System;
using System.Collections.Generic;
using System.Text;

namespace Umi.Core
{
    public static class EndpointManager
    {
        private static List<Endpoint> points = new List<Endpoint>();

        public static bool RegisterEndpoint(Endpoint newItem)
        {
            if (!points.Exists(x=> x.Equals(newItem)))
            {
                points.Add(newItem);
                return true;
            }
            return false;
        }

        public static IList<Endpoint> All() => points;
    }
}
