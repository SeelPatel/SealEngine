using SealEngine.Core.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.ECS
{
    abstract class Component
    {
        public Entity entity;
        public PhysicsSystem physics;
        public abstract void Update();
        public abstract void Init();

        public T GetComponent<T>() where T : Component
        {
            if (entity != null)
            {
                return entity.GetComponent<T>();
            }

            return null;
        }
    }
}
