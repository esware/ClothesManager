using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using DG.Tweening;
using EWGames.Dev.Scripts.Missions;
using TMPro;
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
        }

        private void SetLevelText()
        {
            var currentLevel = GameManager.Instance.currentLevel + 1;
            levelText.text = "DAY " +  currentLevel+ "!";
        }

        void GetCurrentMoney()
        {
            moneyText.text = GameManager.Instance.currentMoney.ToString(CultureInfo.CurrentCulture);
        }

        void ItemSold(ClothingItemData data)
        {
            moneyText.text = GameManager.Instance.currentMoney.ToString(CultureInfo.InvariantCulture);
        }

        private void LoadMissions()
        {
            missions.Clear();
            var gameManager = GameManager.Instance;
            foreach (var mission in gameManager.levels[gameManager.currentLevel].missions)
            {
                var m=Instantiate(missionPrefab, missionsTransform);
                m.transform.localScale = Vector3.zero;
                m.transform.DOScale(Vector3.one, 1f);
                m.Initialize(mission.itemData.itemSprite,ColorMapper.GetColorFromCode(mission.color),mission.targetAmount);
                missions.Add(m);
            }
        }

        public void UpdateMission()
        {
            var gameManager = GameManager.Instance;
            var currentLevelMissions = gameManager.levels[gameManager.currentLevel].missions;

            for (int i = 0; i < missions.Count; i++)
            {
                var mission = missions[i];
                var correspondingLevelMission = currentLevelMissions[i];
                
                if (correspondingLevelMission.isCompleted)
                {
                    mission.transform.DOScale(Vector3.zero, 1f).OnComplete(() =>
                    {
                        Destroy(mission.gameObject);
                        missions.Remove(mission);
                    });
                }
                else
                {
                    var amount = correspondingLevelMission.targetAmount - correspondingLevelMission.currentAmount;
                    mission.UpdateCountText(amount);
                }
            }

            if (AreAllMissionsCompleted(gameManager.currentLevel))
            {
                gameManager.IncreaseLevel();
                SetLevelText();
                LoadMissions();
            }
        }
        
        public bool AreAllMissionsCompleted(int levelIndex)
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

    }
}