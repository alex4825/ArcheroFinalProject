using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public abstract class EntityView : MonoBehaviour
    {
        public void Link(Entity entity)
        {
            entity.Initialized += OnEntityStartedWOrk;
        }

        public virtual void CleanUp(Entity entity)
        {
            entity.Initialized -= OnEntityStartedWOrk;
        }

        protected abstract void OnEntityStartedWOrk(Entity entity);
    }
}