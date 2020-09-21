using Microsoft.Xna.Framework;
using SealEngine.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static SealEngine.Core.Collision.Collider;

namespace SealEngine.Core.Collision
{
    class PhysicsSystem
    {
        // List of all colliders in the current physics system
        private List<Collider> colliders;

        public PhysicsSystem()
        {
            colliders = new List<Collider>();
        }

        // Add collider to the system
        public void AddCollider(Collider collider)
        {
            colliders.Add(collider);
        }
        
        // Do a raycast and return information about the closest hitpoint
        public RaycastHit Raycast(Vector2 start, Vector2 direction)
        {
            Rectangle bounds = GetRaycastBounds(start, direction);

            RaycastHit closest = null;
                        
            foreach (Collider collider in colliders)
            {   
                // Check if collision is possible by checking bounds
                // TODO: add code to check horizontal or vertical intersection of a ray and collider bounds.
                if (collider.GetRectangleBounds().Intersects(bounds) || direction.X == 0 || direction.Y == 0)
                {                   
                    RaycastHit hit = collider.CheckRay(start, direction);

                    if (hit != null)
                    {                        
                        if (closest == null || (hit.distance < closest.distance))
                        {
                            closest = hit;
                        }
                    }
                }
            }

            return closest;
        }

        // Use Seperating Axis Theorem to resolve polygon collision
        // TODO: Get approximate collision point using the minimum seperating vector

        public PolygonHit CheckPolygonCollison(Vector2[] points)
        {
            Rectangle bounds = GetPolygonBounds(points);
            PolygonHit polygonHit = new PolygonHit();
            polygonHit.hit = false;

            Vector2 polygonCenter = GetPolygonCenter(points);
            Vector2 closestPoint = Vector2.Zero;
            bool closestPointSet = false;
            
            foreach (Collider collider in colliders)
            {
                // Check if collision is possible by checking bounds
                if (collider.GetRectangleBounds().Intersects(bounds) || bounds.Width == 0 || bounds.Height == 0)
                {
                    Hit hit = collider.CheckPolygonCollision(points);     
                    
                    if (hit.hit)
                    {
                        polygonHit.hit = true;
                        
                        // TODO: Get hitpoints using SAT result instead of line collisions
                        foreach(Vector2 point in hit.hitpoints)
                        {
                            if (!closestPointSet || Vector2.Distance(polygonCenter, point) < Vector2.Distance(polygonCenter, closestPoint))
                            {
                                closestPointSet = true;
                                closestPoint = point;
                            }
                        }
                    }
                }
            }
            
            polygonHit.hitPoint = closestPoint;

            return polygonHit;
        }

        // Get center of polygon by averaging the position of the verticies
        public Vector2 GetPolygonCenter(Vector2[] points)
        {
            int numVectors = points.Length;
            Vector2 sum = Vector2.Zero;

            foreach(Vector2 point in points)
            {
                sum += point;
            }
            return sum / numVectors;
        }

        // Use Line collisions to determine contact points of two colliding polygons         
        public Vector2[] FindPolygonHitpoints(Line[] poly1, Line[] poly2)
        {
            List<Vector2> collisionPoints = new List<Vector2>();

            foreach (Line line1 in poly1)
            {
                foreach (Line line2 in poly2)
                {
                    LineHit lineHit = PhysicsUtils.CheckLineCollision(line1.p1, line1.p2, line2.p1, line2.p2);         
                    
                    if(lineHit.hit)
                    {
                        collisionPoints.Add(lineHit.hitPoint);
                    }
                }
            }

            return collisionPoints.ToArray();
        }
        

        // Generating Bounds to filter which colliders may be colliding
        //TODO: For colliders, consider caching these bounds for efficiancy

        /*
        Generate collision bounds for raycasts
        */
        private Rectangle GetRaycastBounds(Vector2 start, Vector2 direction)
        {
            Vector2 end = start + direction;

            int xMax = (int)Math.Max(start.X, end.X);
            int yMax = (int)Math.Max(start.Y, end.Y);

            int xMin = (int)Math.Min(start.X, end.X);
            int yMin = (int)Math.Min(start.Y, end.Y);

            return new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        
        /*
        Generate collision bounds for polygons
        */
        public Rectangle GetPolygonBounds(Vector2[] points)
        {
            int xMax = (int)points[0].X;
            int xMin = (int)points[0].X;

            int yMax = (int)points[0].Y;
            int yMin = (int)points[0].Y;

            foreach (Vector2 point in points)
            {
                xMax = Math.Max(xMax, (int)point.X);
                xMin = Math.Min(xMin, (int)point.X);

                yMax = Math.Max(yMax, (int)point.Y);
                yMin = Math.Min(yMin, (int)point.Y);
            }

            return new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
        }
    }
}
