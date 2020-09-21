using SealEngine.Core.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealEngine.Core.ECS
{   
    class Entity
    {
        private Dictionary<String, Component> components;
        public PhysicsSystem physics;

        public String name, tag;

        public LinkedList<Entity> newEntities;

        public Entity(PhysicsSystem physics, String name = "", String tag = "")
        {
            this.name = name;
            this.tag = tag;
            components = new Dictionary<string, Component>();
            this.physics = physics;
        }

        public void InitComponents()
        {
            foreach (String key in components.Keys)
            {
                components[key].Init();
            }
        }

        public void AddComponent(Component component)
        {            
            components[component.GetType().Name] = component;
            component.entity = this;
            component.physics = physics;

            if(component.GetType().IsSubclassOf(typeof(Collider)))
            {
                physics.AddCollider((Collider) component);
            }
        }

        public bool HasComponent<T> () where T : Component
        {
            String type = typeof(T).Name;

            return components.ContainsKey(type);
        }

        public T GetComponent<T>() where T : Component
        {
            String type = typeof(T).Name;            

            if (components.ContainsKey(type))
            {
                return (T) components[type];
            }

            return null;
        }

        public void UpdateComponents()
        {
            foreach(String key in components.Keys) {
                components[key].Update();
            }
        }
    }
}
