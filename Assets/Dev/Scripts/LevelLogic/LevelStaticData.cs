using System;

namespace Dev.Scripts
{
    [Serializable]
    public class LevelStaticData
    {
        public string _name;
        public int Id => int.Parse(_name);
        public Level LevelPrefab;
        public string Description;
    }
}