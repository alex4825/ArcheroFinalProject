using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using System;

namespace Assets._Project.Develop.Runtime.UI.Statistics
{
    public class DefeatPresenter : IPresenter
    {
        private readonly VictoryDefeatCounter _victoryDefeatCounter;
        private readonly IconTextView _defeatView;

        private IDisposable _defeatsDisposable;

        public DefeatPresenter(VictoryDefeatCounter victoryDefeatCounter, IconTextView defeatView)
        {
            _victoryDefeatCounter = victoryDefeatCounter;
            _defeatView = defeatView;
        }

        public void Initialize()
        {
            UpdateView(_victoryDefeatCounter.DefeatCount.Value);
            _defeatsDisposable = _victoryDefeatCounter.DefeatCount.Subscribe(OnDefeatCountChanged);
        }

        public void Dispose()
        {
            _defeatsDisposable.Dispose();
        }

        private void OnDefeatCountChanged(int arg1, int newCount) => UpdateView(newCount);

        private void UpdateView(int count) => _defeatView.SetText("Defeats: " + count.ToString());
    }
}