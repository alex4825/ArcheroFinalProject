using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class ArrowShootSystem : ShootSystem
    {
        private ArrowConfig _config;

        public ArrowShootSystem(ProjectilesFactory projectilesFactory, ArrowConfig config) : base(projectilesFactory)
        {
            _config = config;
        }

        protected override void Shoot()
        {
            ProjectilesFactory.CreateArrow(_config, Entity);
        }
    }
}