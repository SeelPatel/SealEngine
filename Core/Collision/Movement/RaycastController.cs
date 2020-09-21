using Microsoft.Xna.Framework;
using SealEngine.Core.ECS;
using SealEngine.Core.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.Collision.Movement
{
    class RaycastController : Component
    {

        private Transform transform;

        public int width, height;
        public int numHorizontalRaycasts, numVerticalRaycasts;

        public CollisionInfo collisionInfo;

        private RaycastOrgins raycastOrgins;

        public override void Init()
        {
            transform = GetComponent<Transform>();
        }

        public override void Update()
        {

        }

        public void Move(Vector2 velocity)
        {
            raycastOrgins.SetOrgins(transform.position, new Vector2(width, height));
            collisionInfo.Reset();

            HorizontalCollisions(ref velocity);            
            VerticalCollisions(ref velocity);

            transform.position.X += velocity.X;
            transform.position.Y -= velocity.Y;
        }

        public void HorizontalCollisions(ref Vector2 velocity)
        {
            
        }

        public void VerticalCollisions(ref Vector2 velocity)
        {

        }

        public void CalculateRaySpacing()
        {
            Bounds bounds = collider.bounds;
            bounds.Expand(skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

        }

        public struct CollisionInfo
        {
            public bool collidingTop, collidingRight, collidingLeft, collidingBottom;

            public void Reset()
            {
                collidingBottom = false;
                collidingTop = false;

                collidingLeft = false;
                collidingRight = false;
            }
        }

        public struct RaycastOrgins
        {
            public Vector2 leftTop, rightTop, leftBottom, rightBottom;

            public void SetOrgins(Vector2 position, Vector2 size)
            {
                leftTop = position;
                rightTop = position + Vector2.UnitX * size.X;

                leftBottom = position + Vector2.UnitY * size.Y;
                rightBottom = position + Vector2.UnitY * size.Y + Vector2.UnitX * size.X;
            }
        }
    }
}
