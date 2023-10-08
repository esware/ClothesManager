using System;
using System.Collections.Generic;
using EWGames.Dev.Scripts.Missions;
using UnityEngine;

namespace EWGames.Dev.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public List<Level> levels = new List<Level>();
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }

                return _instance;
            }
        }
        public int currentLevel => _currentLevel;
        public float currentMoney => _currentMoney;

        private static GameManager _instance;
        private float _currentMoney;
        private int _currentLevel;
        
        private void Start()
        {
            SignUpEvents();
            
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        private void SignUpEvents()
        {
            ClothingItem.OnItemSold += ItemSold;
        }
        
        private void ItemSold(ClothingItemData data)
        {
            _currentMoney += data.price;
            PlayerPrefs.SetFloat("CurrentMoney",_currentMoney);
        }

        public void IncreaseLevel()
        {
            PlayerPrefs.SetInt("CurrentLevel",PlayerPrefs.GetInt("CurrentLevel")+1);
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
    }

}