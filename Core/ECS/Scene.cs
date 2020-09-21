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

        // Initialize Scene. Scene is setup here
        public abstract void Init();

        // Render Scene. Everything is rendered here
        public abstract void Render();

        // Update Scene. Logic is updated here
        public abstract void Update();

        // Update all the entities of this scene
        public void UpdateEntities()
        {
            foreach (Entity entity in entities)
            {
                entity.UpdateComponents();
            }
        }       
        
        // Initialize all the entities of this scene
        public void InitEntities()
        {
            foreach (Entity entity in entities)
            {
                entity.InitComponents();
            }
        }
    }
}
