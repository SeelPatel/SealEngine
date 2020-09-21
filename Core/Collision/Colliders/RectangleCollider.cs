using Microsoft.Xna.Framework;
using SealEngine.Core.ECS.Components;
using SealEngine.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.Collision.Colliders
{
    class RectangleCollider : Collider
    {
        private Rectangle bounds;

        public Transform transform;

        public Vector2 size;

        public override void Init()
        {
            transform = GetComponent<Transform>();
            UpdateBounds();
        }

        public override void Update()
        {
            UpdateBounds();            
        }

        private void UpdateBounds()
        {
            bounds.X = (int)transform.position.X;
            bounds.Y = (int)transform.position.Y;

            bounds.Width = (int)size.X;
            bounds.Height = (int)size.Y;
        }

        // Check raycast collision

        public override RaycastHit CheckRay(Vector2 start, Vector2 direction)
        {
            Vector2[] points = GetPoints();

            Vector2 end = start + direction;

            LineHit closest = PhysicsUtils.CheckLineCollision(points[3], points[0], start, end);

            for (int i = 0; i <= 2; i++)
            {
                Vector2 p1 = points[i];
                Vector2 p2 = points[i+1];

                LineHit hit = PhysicsUtils.CheckLineCollision(p1, p2, start, end);

                if (hit.hit)
                {
                    if(!closest.hit || Vector2.Distance(hit.hitPoint, start) < Vector2.Distance(closest.hitPoint, start))
                    {
                        closest = hit;
                    }
                }
            }

            if (!closest.hit)
            {
                return null;
            }

            RaycastHit raycastHit = new RaycastHit(closest.hitPoint, entity, Vector2.Distance(closest.hitPoint, start));

            return raycastHit;
        }

        // check collisoin of polygon with this rectangle using SAT

        public override Hit CheckPolygonCollision(Vector2[] points)
        {
            Line[] polygonLines = LinesFromPoints(points);

            Line[] rectLines = LinesFromPoints(GetPoints());

            return CheckSATCollision(polygonLines, rectLines);
        }

        // Get the points of the rectangle

        public Vector2[] GetPoints()
        {           
            return new Vector2[]{
                new Vector2(bounds.X, bounds.Y), new Vector2(bounds.X + bounds.Width, bounds.Y), 
                 new Vector2(bounds.X + bounds.Width, bounds.Y + bounds.Height), new Vector2(bounds.X, bounds.Y + bounds.Height)};
        }

        public override void DebugDraw()
        {
            throw new NotImplementedException();
        }

        // Return the bounds of the rectangle
        public override Rectangle GetRectangleBounds()
        {
            return bounds;
        }    
    }
}
