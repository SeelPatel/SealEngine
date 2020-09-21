using Microsoft.Xna.Framework;
using SealEngine.Core.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SealEngine.Core.Utils;

namespace SealEngine.Core.Collision
{
    abstract class Collider : Component
    {
        public abstract RaycastHit CheckRay(Vector2 start, Vector2 direction);

        public abstract Hit CheckPolygonCollision(Vector2[] points);

        public abstract Rectangle GetRectangleBounds();

        public abstract void DebugDraw();

        // Raycast Methods

        public Rectangle GetRaycastBounds(Vector2 start, Vector2 direction)
        {
            Vector2 end = start + direction;

            int xMax = (int)Math.Max(start.X, end.X);
            int yMax = (int)Math.Max(start.Y, end.Y);

            int xMin = (int)Math.Min(start.X, end.X);
            int yMin = (int)Math.Min(start.Y, end.Y);

            return new Rectangle(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        public Line[] LinesFromPoints(Vector2[] points)
        {
            int numPoints = points.Length;
            if (numPoints < 2)
            {
                return new Line[]{ };
            }

            Line[] polygonLines = new Line[numPoints];

            Line line1 = new Line();
            line1.p1 = points[numPoints - 1];
            line1.p2 = points[0];

            polygonLines[0] = line1;

            for (int i = 0; i < numPoints-1; i++)
            {
                Line line = new Line();
                line.p1 = points[i];
                line.p2 = points[i + 1];
                polygonLines[i+1] = line;
            }

            return polygonLines;
        }


        // Polygon Utils

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

        // SAT Methods

        public Hit CheckSATCollision(Line[] poly1, Line[] poly2)
        {
            Hit hit = new Hit();
            hit.hit = false;
            hit.hitpoints = new Vector2[0];

            Vector2[] poly1Axes = GenerateSATAxes(poly1);
            Vector2[] poly2Axes = GenerateSATAxes(poly2);

            if(CheckSATAxes(poly1Axes, poly1, poly2) && CheckSATAxes(poly2Axes, poly1, poly2))
            {
                hit.hit = true;
                //hit.hitpoints = FindPolygonHitpoints(poly1, poly2);
            }

            return hit;
        }

        public Vector2[] FindPolygonHitpoints(Line[] poly1, Line[] poly2)
        {
            List<Vector2> collisionPoints = new List<Vector2>();

            foreach (Line line1 in poly1)
            {
                foreach (Line line2 in poly2)
                {
                    LineHit lineHit = PhysicsUtils.CheckLineCollision(line1.p1, line1.p2, line2.p1, line2.p2);

                    if (lineHit.hit)
                    {
                        collisionPoints.Add(lineHit.hitPoint);
                    }
                }
            }

            return collisionPoints.ToArray();
        }

        private bool CheckSATAxes(Vector2[] axes, Line[] poly1, Line[] poly2)
        {
            foreach(Vector2 axis in axes)
            {
                Interval interval1 = GetSeperatingAxisInterval(axis, poly1);
                Interval interval2 = GetSeperatingAxisInterval(axis, poly2);

                if (!IntervalsIntersect(interval1, interval2))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IntervalsIntersect(Interval x, Interval y)
        {
            // x1 <= y2 && y1 <= x2

            return (x.a <= y.b) && (y.a <= x.b);
        }

        private Interval GetSeperatingAxisInterval(Vector2 axis, Line[] lines)
        {            
            Interval interval = new Interval();
            bool set = false;

            interval.a = 0;
            interval.b = 0;

            for(int i = 0; i < lines.Length; i++)
            {
                Line line = lines[i];
                
                float x = ScalarProjection(axis, line.p1);
                float y = ScalarProjection(axis, line.p2);

                float min = Math.Min(x, y);
                float max = Math.Max(x, y);

                if (set)
                {
                    interval.a = Math.Min(interval.a, min);
                    interval.b = Math.Max(interval.b, max);
                }
                else
                {
                    interval.a = min;
                    interval.b = max;
                    set = true;
                }                                
            }

            return interval;
        }

        private Vector2[] GenerateSATAxes(Line[] lines)
        {
            Vector2[] axes = new Vector2[lines.Length];

            for(int i = 0; i < lines.Length; i++)
            {
                Vector2 dir = lines[i].p2 - lines[i].p1;
                Vector2 axis = PerpendicularVector(dir);
                axes[i] = axis;
            }

            return axes;
        }

        public Vector2 PerpendicularVector(Vector2 vector2)
        {
            return new Vector2(vector2.Y, -vector2.X);
        }

        private float ScalarProjection(Vector2 axis, Vector2 point)
        {
            return Vector2.Dot(axis, point) / axis.Length();
        }

        // Structs

        public struct Hit
        {
            public bool hit;
            public Vector2[] hitpoints;
        }

        public struct LineHit
        {
            public Vector2 hitPoint;
            public bool hit;
        }

        public struct Line
        {
            public Vector2 p1;
            public Vector2 p2;
        }

        public struct Interval
        {
            public float a;
            public float b;
        }

    }
}
