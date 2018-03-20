using System.Collections.Generic;
using System.Threading.Tasks;

namespace Umi.Core
{
    public static class EndpointManager
    {
        private static List<Endpoint> points = new List<Endpoint>();

        public static bool RegisterEndpoint(Endpoint newItem)
        {
            if (!points.Exists(x => x.Equals(newItem)))
            {
                points.Add(newItem);
                return true;
            }
            return false;
        }

        public static async Task<IList<Endpoint>> All()
        {
            foreach (var item in points)
            {
                await item.DoTest();
            }
            return points;
        }
    }
}
