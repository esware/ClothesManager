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
            foreach (Mission mission in GameManager.Instance.levels[GameManager.Instance.currentLevel].missions)
            {
                if (mission.itemData.itemSprite == itemData.itemSprite  && mission.color==itemData.color && !mission.isCompleted)
                {
                    mission.UpdateProgress(1);
                }
            }
        }
    }
}