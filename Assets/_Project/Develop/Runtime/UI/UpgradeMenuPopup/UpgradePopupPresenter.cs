using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using System;

namespace Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup
{
    public class UpgradePopupPresenter : PopupPresenterBase
    {
        private UpgradePopupView _upgradePopupView;

        public UpgradePopupPresenter(
            UpgradePopupView upgradePopupView,
            ICoroutinesPerformer coroutinesPerformer) : base(coroutinesPerformer)
        {
            _upgradePopupView = upgradePopupView;
        }

        protected override PopupViewBase PopupView => _upgradePopupView;
    }
}