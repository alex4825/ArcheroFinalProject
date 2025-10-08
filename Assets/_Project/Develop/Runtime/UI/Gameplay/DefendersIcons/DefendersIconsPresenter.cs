using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders;
using Assets._Project.Develop.Runtime.UI.Core;
using System;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.DefendersIcons
{
    public class DefendersIconsPresenter : IPresenter
    {
        public event Action<DefenderConfig> IconClicked;

        private readonly ViewsFactory _viewsFactory;
        private DefendersIconsListView _iconsListView;

        public DefendersIconsPresenter(
            ViewsFactory viewsFactory,
            DefendersIconsListView iconsListView)
        {
            _viewsFactory = viewsFactory;
            _iconsListView = iconsListView;
        }

        public void Initialize()
        {
            foreach (DefenderConfig defenderConfig in _iconsListView.DefendersConfigs)
            {
                DefenderIconView defenderView = _viewsFactory.Create<DefenderIconView>(ViewIDs.DefenderIconView);
                _iconsListView.Add(defenderView);

                defenderView.SetConfig(defenderConfig);
                defenderView.SetText(defenderConfig.Cost.ToString());
                defenderView.SetIcon(defenderConfig.Icon);

                defenderView.Clicked += OnIconClicked;
            }
        }

        public void Dispose()
        {
            foreach (DefenderIconView view in _iconsListView.Elements)
            {
                view.Clicked -= OnIconClicked;
                _viewsFactory.Release(view);
            }
        }

        private void OnIconClicked(DefenderIconView clickedIconView)
        {
            clickedIconView.SetBackgroundColor(_iconsListView.SelectedBackgroundColor);

            foreach (DefenderIconView iconView in _iconsListView.Elements)
            {
                if (iconView == clickedIconView)
                    continue;

                iconView.SetBackgroundColor(_iconsListView.DefaultBackgroundColor);
            }

            IconClicked?.Invoke(clickedIconView.DefenderConfig);
        }
    }
}