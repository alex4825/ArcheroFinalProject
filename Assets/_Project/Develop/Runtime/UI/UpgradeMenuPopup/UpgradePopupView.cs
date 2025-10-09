using Assets._Project.Develop.Runtime.Configs.Meta.Upgrade;
using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup
{
    public class UpgradePopupView : PopupViewBase
    {
        [field:SerializeField] public Transform CardsContainer;
        [field:SerializeField] public UpgradesConfig UpgradesConfig;
    }
}