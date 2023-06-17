using System;

namespace Data
{
    [Serializable]
    public class ZombiesData
    {
        public bool[] Family;

        public ZombiesData() =>
            Family = new bool[3];

        public bool CheckAllCharacters()
        {
            foreach (bool character in Family)
            {
                if (character != true)
                    return false;
            }

            return true;
        }
    }
}