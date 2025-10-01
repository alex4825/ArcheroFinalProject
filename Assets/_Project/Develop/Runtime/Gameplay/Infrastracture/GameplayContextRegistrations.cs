using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.States;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.UI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagement;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using UnityEngine;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesDroppingFeature;

namespace Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture
{
    public class GameplayContextRegistrations
    {
        private static GameplayInputArgs _inputArgs;

        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            Debug.Log("Процесс регистрации сервисов на сцене геймплея");

            _inputArgs = args;

            container.RegisterAsSingle(CreateEntitiesFactory);

            container.RegisterAsSingle(CreateEntitiesLifeContext);

            container.RegisterAsSingle(CreateMonoEntitiesFactory).NonLazy();

            container.RegisterAsSingle(CreateCollidersRegistryService);

            container.RegisterAsSingle(CreateBrainsFactory);

            container.RegisterAsSingle(CreateAIBrainsContext);

            container.RegisterAsSingle<IInputService>(CreateDesktopInput);

            container.RegisterAsSingle(CreateEnemiesFactory);

            container.RegisterAsSingle(CreateMainHeroFactory);

            container.RegisterAsSingle(CreateStagesFactory);

            container.RegisterAsSingle(CreateStageProviderService);

            container.RegisterAsSingle(CreatePreparationTriggerService);

            container.RegisterAsSingle(CreateMainHeroHolderService).NonLazy();

            container.RegisterAsSingle(CreateGameplayStatesFactory);

            container.RegisterAsSingle(CreateGameplayStatesContext);

            container.RegisterAsSingle(CreateGameplayUIRoot).NonLazy();

            container.RegisterAsSingle(CreateGameplayScreenPresenter).NonLazy();

            container.RegisterAsSingle(CreateGameplayPresentersFactory);

            container.RegisterAsSingle(CreateGameplayPopupService);

            container.RegisterAsSingle(CreateAbilityFactory);

            container.RegisterAsSingle(CreateAbilityDropingRulesService);

            container.RegisterAsSingle(CreateAbilityDropService);
        }

        private static AbilityDropService CreateAbilityDropService(DIContainer c)
        {
            return new AbilityDropService(
                c.Resolve<ConfigsProviderService>().GetConfig<AbilitiesConfigsContainer>(),
                c.Resolve<AbilityDropingRulesService>());
        }

        private static AbilityDropingRulesService CreateAbilityDropingRulesService(DIContainer c)
        {
            return new AbilityDropingRulesService();
        }

        private static AbilityFactory CreateAbilityFactory(DIContainer container)
            => new AbilityFactory(container);

        private static GameplayPopupService CreateGameplayPopupService(DIContainer container)
        {
            return new GameplayPopupService(
                container.Resolve<ViewsFactory>(),
                container.Resolve<ProjectPresentersFactory>(),
                container.Resolve<GameplayUIRoot>(),
                container.Resolve<GameplayPresentersFactory>());
        }

        private static GameplayUIRoot CreateGameplayUIRoot(DIContainer container)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = container.Resolve<ResourcesAssetsLoader>();

            GameplayUIRoot gameplayUIRootPrefab = resourcesAssetsLoader
                .Load<GameplayUIRoot>("UI/Gameplay/GameplayUIRoot");

            return Object.Instantiate(gameplayUIRootPrefab);
        }

        private static GameplayScreenPresenter CreateGameplayScreenPresenter(DIContainer container)
        {
            GameplayUIRoot uiRoot = container.Resolve<GameplayUIRoot>();

            GameplayScreenView view = container
                .Resolve<ViewsFactory>()
                .Create<GameplayScreenView>(ViewIDs.GameplayScreen, uiRoot.HUDLayer);

            GameplayScreenPresenter presenter = container
                .Resolve<GameplayPresentersFactory>()
                .CreateGameplayScreenPresenter(view);

            return presenter;
        }

        private static GameplayPresentersFactory CreateGameplayPresentersFactory(DIContainer container)
        {
            return new GameplayPresentersFactory(container, _inputArgs);
        }

        private static GameplayStatesContext CreateGameplayStatesContext(DIContainer container)
            => new GameplayStatesContext(container.Resolve<GameplayStatesFactory>().CreateGameplayStateMachine(_inputArgs));

        private static GameplayStatesFactory CreateGameplayStatesFactory(DIContainer container)
            => new GameplayStatesFactory(container);

        private static MainHeroHolderService CreateMainHeroHolderService(DIContainer container)
            => new MainHeroHolderService(container.Resolve<EntitiesLifeContext>());

        private static PreparationTriggerService CreatePreparationTriggerService(DIContainer container)
            => new PreparationTriggerService(container.Resolve<EntitiesFactory>(), container.Resolve<EntitiesLifeContext>());

        private static StageProviderService CreateStageProviderService(DIContainer container)
            => new StageProviderService(
                container.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>().GetBy(_inputArgs.LevelNumber),
                container.Resolve<StagesFactory>());

        private static StagesFactory CreateStagesFactory(DIContainer container)
            => new StagesFactory(container);

        private static EnemiesFactory CreateEnemiesFactory(DIContainer container)
            => new EnemiesFactory(container);

        private static MainHeroFactory CreateMainHeroFactory(DIContainer container)
            => new MainHeroFactory(container);

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
