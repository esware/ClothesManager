using System;
using System.Collections.Generic;
using EWGames.Dev.Scripts.Missions;
using UnityEngine;

namespace EWGames.Dev.Scripts
{
    public struct GameEvents
    {
        public static Action<int> OnLevelCompleted;
        public static Action<int> OnMachineSold;
    }
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
        public int currentMoney => _currentMoney;

        private static GameManager _instance;
        private int _currentMoney;
        private int _currentLevel;

        private void Awake()
        {
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            _currentMoney = PlayerPrefs.GetInt("CurrentMoney");
            
#if UNITY_EDITOR
            Debug.Log($"Current Level: {_currentLevel.ToString()}");
            Debug.Log($"Current Money: {_currentMoney.ToString()}");
#endif
        }

        private void Start()
        {
            SignUpEvents();
        }
        private void SignUpEvents()
        {
            ClothingItem.OnItemSold += ItemSold;
        }
        
        private void ItemSold(int price)
        {
            _currentMoney += price;
            PlayerPrefs.SetInt("CurrentMoney",_currentMoney);
        }
        public void BuyMachine(int price)
        {
            _currentMoney -= price;
            GameEvents.OnMachineSold?.Invoke(price);
        }

        public void IncreaseLevel()
        {
            PlayerPrefs.SetInt("CurrentLevel",PlayerPrefs.GetInt("CurrentLevel")+1);
            _currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            GameEvents.OnLevelCompleted?.Invoke(_currentLevel);
        }

        
    }

}