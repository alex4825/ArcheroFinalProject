using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class CannonballShootSystem : ShootSystem
    {
        private CannonballConfig _config;

        public CannonballShootSystem(ProjectilesFactory projectilesFactory, CannonballConfig config) : base(projectilesFactory)
        {
            _config = config;
        }

        protected override ReactiveEvent EventToShoot => Entity.AttackCooldownIsOverEvent;

        protected override void OnShoot()
        {
            ProjectilesFactory.CreateСannonball(_config, Entity);
        }
    }
}