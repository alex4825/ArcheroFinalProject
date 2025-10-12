namespace Assets._Project.Develop.Runtime.Gameplay.Waves
{
    public class WaveResult
    {
        public WaveResult(bool isWin, int defeatedEnemiesCount)
        {
            IsWin = isWin;
            KilledEnemiesCount = defeatedEnemiesCount;
        }

        public bool IsWin { get; }
        public int KilledEnemiesCount { get; }
    }
}