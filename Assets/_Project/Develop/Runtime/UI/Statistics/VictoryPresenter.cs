using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using System;

namespace Assets._Project.Develop.Runtime.UI.Statistics
{
    public class VictoryPresenter : IPresenter
    {
        private readonly VictoryDefeatCounter _victoryDefeatCounter;
        private readonly IconTextView _victoryView;

        private IDisposable _victoriesDisposable;

        public VictoryPresenter(VictoryDefeatCounter victoryDefeatCounter, IconTextView victoryView)
        {
            _victoryDefeatCounter = victoryDefeatCounter;
            _victoryView = victoryView;
        }

        public void Initialize()
        {
            UpdateView(_victoryDefeatCounter.WinCount.Value);
            _victoriesDisposable = _victoryDefeatCounter.WinCount.Subscribe(OnVictoryCountChanged);
        }

        public void Dispose()
        {
            _victoriesDisposable.Dispose();
        }

        private void OnVictoryCountChanged(int arg1, int newCount) => UpdateView(newCount);

        private void UpdateView(int count) => _victoryView.SetText("Victories: " + count.ToString());
    }
}