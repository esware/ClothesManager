using System.Collections.Generic;
using EWGames.Dev.Scripts.Missions;

namespace EWGames.Dev.Scripts
{
    [System.Serializable]
    public class Level
    {
        public int levelNumber;
        public List<Mission> missions = new List<Mission>();
    }
}