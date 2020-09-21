using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SealEngine.Core.Collision.Collider;

namespace SealEngine.Core.Utils
{
    class PhysicsUtils
    {
        // Check for the collision of two lines using cramers rule
        public static LineHit CheckLineCollision(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
        {
            Vector2 a = p2 - p1;
            Vector2 b = q2 - q1;

            Vector2 c = q1 - p1;

            // Check if parallel;

            LineHit linehit = new LineHit();

            linehit.hitPoint = Vector2.Zero;
            linehit.hit = false;

            if (SealMath.Approximately(a.X / b.X, a.Y / b.Y))
            {
                return linehit;
            }

            // Use Cramers Rule to solve 

            float D = (a.X * b.Y) - (a.Y * b.X);

            float D_x = (c.X * b.Y) - (c.Y * b.X);

            float D_y = (a.X * c.Y) - (a.Y * c.X);

            float x = D_x / D;

            float y = D_y / D;


            if ((0 <= x && x <= 1) && (0 <= y && y <= 1))
            {
                linehit.hitPoint = p1 + x * a;
                linehit.hit = true;
            }

            return linehit;
        }
    }
}
