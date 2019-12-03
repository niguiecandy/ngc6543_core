using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGC6543
{
    public static class BoundsExtensions 
    {

        public static Vector3 GetRandomInside(this Bounds b)
        {
            Vector3 extents = b.extents;

            return b.center + new Vector3(Random.Range(-extents.x, extents.x), 
                                          Random.Range(-extents.y, extents.y), 
                                          Random.Range(-extents.z, extents.z));
        }
    }
}
