using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Wallet;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture.Meta.Infrastracture
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            Debug.Log("Процесс регистрации сервисов на сцене главного меню");

            container.RegisterAsSingle(CreateWalletPresenter).NonLazy();
        }

        public static WalletPresenter CreateWalletPresenter(DIContainer container)
        {
            IconTextListView walletView = Object.FindObjectOfType<IconTextListView>();

            WalletPresenter walletPresenter = container.Resolve<ProjectPresentersFactory>().CreateWalletPresenter(walletView);

            return walletPresenter;
        }
    }
}
