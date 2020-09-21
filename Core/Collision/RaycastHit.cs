using Microsoft.Xna.Framework;
using SealEngine.Core.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.Collision
{
    // Class to store information about a raycast collision
    class RaycastHit
    {
        public Vector2 point;
        public Entity entity;
        public float distance;

        public RaycastHit(Vector2 point, Entity entity, float distance)
        {
            this.point = point;
            this.entity = entity;
            this.distance = distance;
        }
    }
}
