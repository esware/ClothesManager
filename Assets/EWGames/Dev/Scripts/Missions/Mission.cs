﻿using UnityEngine;

namespace EWGames.Dev.Scripts.Missions
{

    [System.Serializable]
    public class Mission
    {
        public ClothingItemData itemData;
        public int targetAmount;
        public ColorCode color;
        public bool isCompleted;
        public int currentAmount;
        

        public void UpdateProgress(int amount)
        {
            currentAmount += amount;
            if (currentAmount >= targetAmount)
            {
                isCompleted = true;
            }
        }
    }
}