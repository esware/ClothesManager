using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using DG.Tweening;
using EWGames.Dev.Scripts.Missions;
using EWGames.Dev.Scripts.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace EWGames.Dev.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public Image moneyImage;
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI levelText;
        public Transform missionsTransform;
        public MissionUI missionPrefab;

        public List<MissionUI> missions = new List<MissionUI>();

        private void Start()
        {
            SignUpEvents();
            GetCurrentMoney();
            LoadMissions();
            SetLevelText();
        }

        private void SignUpEvents()
        {
            ClothingItem.OnItemSold += ItemSold;
            GameEvents.OnMachineSold += ItemSold;
        }

        private void SetLevelText()
        {
            var levelIndex = GameManager.Instance.currentLevel + 1;
            levelText.text = "DAY " + levelIndex + "!";
        }

        void GetCurrentMoney()
        {
            var money = GameManager.Instance.currentMoney;
            moneyText.text = ChangeTextFormat(money);
        }

        void ItemSold(int price)
        {
            var money = GameManager.Instance.currentMoney;
            moneyText.text = ChangeTextFormat(money);
        }

        #region Mission

        private async Task LoadMissions()
        {
            missions.Clear();
            var layoutGroup = missionsTransform.GetComponent<HorizontalLayoutGroup>();
            var gameManager = GameManager.Instance;
            
            foreach (var mission in gameManager.levels[gameManager.currentLevel].missions)
            {
                var m=Instantiate(missionPrefab, missionsTransform);
                
                m.transform.localScale = Vector3.zero;
                m.transform.localPosition=Vector3.zero;
                m.transform.DOScale(Vector3.one, 1f);
                
                m.Initialize(mission.itemData.itemSprite,ColorMapper.GetColorFromCode(mission.color),mission.targetAmount);
                missions.Add(m);
                
                layoutGroup.enabled = false;
                await Task.Delay(1000);
                layoutGroup.enabled = true;
            }
        }

        public void UpdateMission()
        {
            var gameManager = GameManager.Instance;
            var currentLevelMissions = gameManager.levels[gameManager.currentLevel].missions;

            for (int i = 0; i < currentLevelMissions.Count; i++)
            {
                var mission = missions[i];
                if (currentLevelMissions[i].isCompleted)
                {
                    mission.transform.DOScale(Vector3.one*1.5f, 0.5f).OnComplete(() =>
                    {
                        mission.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
                        {
                            mission.gameObject.SetActive(false);
                            
                        });
                    });
                }
                else
                {
                    var amount = currentLevelMissions[i].targetAmount - currentLevelMissions[i].currentAmount;
                    mission.UpdateCountText(amount);
                }
                
                if (AreAllMissionsCompleted(gameManager.currentLevel))
                {
                    gameManager.levels[gameManager.currentLevel].missions.Clear();
                    gameManager.IncreaseLevel();
                    SetLevelText();
                
                    foreach (var m in missions)
                    {
                        Destroy(m.gameObject);
                    }

                    LoadMissions();
                    return;
                }
            }
        }
        
        private bool AreAllMissionsCompleted(int levelIndex)
        {
            var gameManager = GameManager.Instance;
            var levelMissions = gameManager.levels[levelIndex].missions;

            foreach (var mission in levelMissions)
            {
                if (!mission.isCompleted)
                {
                    return false; 
                }
            }

            return true;
        }

        #endregion

        
        private string ChangeTextFormat(int money)
        {
            if (money >= 1000)
            {
                return (money / 1000f).ToString("F1") + "k $";
            }
            return money.ToString();
        }


    }
}