using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.ECS.Components.Graphics
{
    // Component that holds the sprite to be rendered by SpriteRenderer
    class Sprite : Component
    {
        public Texture2D currentTexture;
        public Transform transform;
        public int order = 0;

        public Rectangle bounds;

        public override void Init()
        {
            transform = GetComponent<Transform>();
            bounds = new Rectangle();
            SetBounds();
        }

        public override void Update()
        {
            SetBounds();
        }

        private void SetBounds()
        {
            bounds.X = (int)transform.position.X;
            bounds.Y = (int)transform.position.Y;
            bounds.Width = currentTexture.Width;
            bounds.Height = currentTexture.Height;
        }

        public bool CheckBounds(Rectangle rectangle)
        {
            return bounds.Intersects(rectangle);
        }
    }
}
