using EWGames.Dev.Scripts.Missions;
using UnityEngine;

namespace EWGames.Dev.Scripts.ShopItem
{
    public class Shop : MonoBehaviour
    {
        private static Shop _instance;
        public static Shop Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Shop>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("Shop");
                        _instance = go.AddComponent<Shop>();
                    }
                }

                return _instance;
            }
        }
        
        public void SellItem(ClothingItemData itemData)
        {
            foreach (Level level in GameManager.Instance.levels)
            {
                foreach (Mission mission in level.missions)
                {
                    if (mission.itemData==itemData && mission.color==itemData.color && !mission.isCompleted)
                    {
                        mission.UpdateProgress(1);
                    }
                }
            }
        }
    }
}