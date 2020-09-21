using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SealEngine.Core.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.ECS
{
    abstract class Scene
    {
        public List<Entity> entities;

        public SpriteBatch spriteBatch;

        public GraphicsDeviceManager graphics;

        public PhysicsSystem physics;

        public Scene(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            entities = new List<Entity>();
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;
            this.physics = new PhysicsSystem();
        }

        public abstract void Init();

        public abstract void Render();

        public abstract void Update();

        public void UpdateEntities()
        {
            foreach (Entity entity in entities)
            {
                entity.UpdateComponents();
            }
        }       
        
        public void InitEntities()
        {
            foreach (Entity entity in entities)
            {
                entity.InitComponents();
            }
        }
    }
}
