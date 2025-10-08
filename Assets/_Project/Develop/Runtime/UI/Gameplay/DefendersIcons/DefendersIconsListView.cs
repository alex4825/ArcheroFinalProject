using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.DefendersIcons
{
    public class DefendersIconsListView : ElementsListView<DefenderIconView>
    {
        [field: SerializeField] public List<DefenderConfig> DefendersConfigs;
    }
}
