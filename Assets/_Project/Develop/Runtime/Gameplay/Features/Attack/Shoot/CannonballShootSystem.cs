using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class CannonballShootSystem : ShootSystem
    {
        private CannonballConfig _config;

        public CannonballShootSystem(ProjectilesFactory projectilesFactory, CannonballConfig config) : base(projectilesFactory)
        {
            _config = config;
        }

        protected override void Shoot()
        {
            ProjectilesFactory.CreateСannonball(_config, Entity);
        }
    }
}