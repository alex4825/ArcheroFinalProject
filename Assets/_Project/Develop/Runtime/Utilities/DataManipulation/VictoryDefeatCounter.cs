using Assets._Project.Develop.Runtime.Utilities.DataManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Utilities.DataManipulation
{
    public class VictoryDefeatCounter : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private ReactiveVariable<int> _victoryCount;
        private ReactiveVariable<int> _defeatCount;

        public VictoryDefeatCounter(PlayerDataProvider playerDataProvider)
        {
            _victoryCount = new ReactiveVariable<int>();
            _defeatCount = new ReactiveVariable<int>();

            playerDataProvider.RegisterWriter(this);
            playerDataProvider.RegisterReader(this);
        }

        public IReadonlyVariable<int> WinCount => _victoryCount;
        public IReadonlyVariable<int> DefeatCount => _defeatCount;

        public void AddVictory() => _victoryCount.Value++;

        public void AddDefeat() => _defeatCount.Value++;

        public void Reset()
        {
            _victoryCount.Value = 0;
            _defeatCount.Value = 0;
        }

        public void ReadFrom(PlayerData data)
        {
            _victoryCount.Value = data.VictoryCount;
            _defeatCount.Value = data.DefeatCount;
        }

        public void WriteTo(PlayerData data)
        {
            data.VictoryCount = _victoryCount.Value;
            data.DefeatCount = _defeatCount.Value;
        }
    }
}