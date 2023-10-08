﻿using UnityEngine;
using NaughtyAttributes;

namespace EWGames.Dev.Scripts
{
    public enum MachineType
    {
        SewingMachine,
        PaintMachine
    }
    
    [CreateAssetMenu(fileName = "",menuName = "EwGames/Data/MachineData")]
    public class MachineData:ScriptableObject
    {
        [SerializeField] public MachineType machineType;
        [HideIf("machineType",MachineType.SewingMachine)]
        [SerializeField] public ColorCode color;
        
        [SerializeField] public float operationTime;
        [SerializeField] public float earnedMoney;
        [SerializeField] public float unlockedPrice;
        [SerializeField] public int unlockedLevel;
        [SerializeField] private Sprite lockedSpite;



    }
}