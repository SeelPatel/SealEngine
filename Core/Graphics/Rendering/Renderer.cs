using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SealEngine.Core.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.Graphics.Rendering
{
    abstract class Renderer
    {    
        public abstract void RenderEntities(List<Entity> entities, Camera camera, RenderTarget2D target, SpriteBatch spriteBatch);

        public abstract void RenderEntity(Entity entity, Camera camera, RenderTarget2D target, SpriteBatch spriteBatch);
    }
}
