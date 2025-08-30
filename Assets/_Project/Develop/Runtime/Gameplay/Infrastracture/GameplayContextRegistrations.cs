using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            Debug.Log("Процесс регистрации сервисов на сцене геймплея");

            container.RegisterAsSingle(CreateEntitiesFactory);

            container.RegisterAsSingle(CreateEntitiesLifeContext);

            container.RegisterAsSingle(CreateMonoEntitiesFactory).NonLazy();

            container.RegisterAsSingle(CreateCollidersRegistryService);

            container.RegisterAsSingle(CreateBrainsFactory);

            container.RegisterAsSingle(CreateAIBrainsContext);

            container.RegisterAsSingle<IInputService>(CreateDesktopInput);
        }

        private static DesktopInput CreateDesktopInput(DIContainer container)
            => new DesktopInput();

        private static AIBrainsContext CreateAIBrainsContext(DIContainer container)
            => new AIBrainsContext();

        private static BrainsFactory CreateBrainsFactory(DIContainer container)
            => new BrainsFactory(container);

        private static CollidersRegistryService CreateCollidersRegistryService(DIContainer container)
            => new CollidersRegistryService();

        private static MonoEntitiesFactory CreateMonoEntitiesFactory(DIContainer container)
            => new MonoEntitiesFactory(
                container.Resolve<ResourcesAssetsLoader>(), 
                container.Resolve<EntitiesLifeContext>(),
                container.Resolve<CollidersRegistryService>());

        private static EntitiesLifeContext CreateEntitiesLifeContext(DIContainer container)
            => new EntitiesLifeContext();

        private static EntitiesFactory CreateEntitiesFactory(DIContainer container)
            => new EntitiesFactory(container);
    }
}
