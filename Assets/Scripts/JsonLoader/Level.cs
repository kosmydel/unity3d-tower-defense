using System;

namespace JsonLoader
{
    [System.Serializable]
    public class Level
    {
        public String name;
        public int startingMoney;
        public int startingHealth;
        public WaveJson[] waves;
    }
}