using System;

namespace Data
{
    [Serializable]
    public class AchievementsData
    {
        public bool[] Statuses;

        public AchievementsData() =>
            Statuses = new bool[11];
    }
}