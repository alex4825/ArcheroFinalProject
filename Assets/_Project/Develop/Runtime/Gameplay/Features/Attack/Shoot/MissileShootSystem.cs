using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class MissileShootSystem : ShootSystem
    {
        private MissileConfig _config;

        public MissileShootSystem(ProjectilesFactory projectilesFactory, MissileConfig config) : base(projectilesFactory)
        {
            _config = config;
        }

        protected override void Shoot()
        {
            ProjectilesFactory.CreateMissile(_config, Entity);
        }
    }
}