using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SealEngine.Core.ECS;
using SealEngine.Core.ECS.Components;
using SealEngine.Core.ECS.Components.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.Graphics.Rendering
{
    class SpriteRenderer : Renderer
    {        
        public override void RenderEntities(List<Entity> entities, Camera camera, RenderTarget2D target, SpriteBatch spriteBatch)
        {
            foreach(Entity entity in entities)
            {
                RenderEntity(entity, camera, target, spriteBatch);
            }
        }

        public override void RenderEntity(Entity entity, Camera camera, RenderTarget2D target, SpriteBatch spriteBatch)
        {
            Sprite sprite = entity.GetComponent<Sprite>();

            Vector2 cameraPos = new Vector2(camera.bounds.X, camera.bounds.Y);

            if(sprite != null)
            {                
                Transform t = sprite.transform;
                
                if(sprite.CheckBounds(camera.bounds))
                {                    
                    spriteBatch.Draw(sprite.currentTexture, t.position - cameraPos, Color.Red);
                }
            }
        }
    }
}
