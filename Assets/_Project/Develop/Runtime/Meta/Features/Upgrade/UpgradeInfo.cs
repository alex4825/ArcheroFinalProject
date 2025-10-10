namespace Assets._Project.Develop.Runtime.Meta.Features.Upgrade
{
    public struct UpgradeInfo
    {
        public StatTypes Type;
        public float Koef;

        public UpgradeInfo(StatTypes type, float koef)
        {
            Type = type;
            Koef = koef;
        }
    }
}
