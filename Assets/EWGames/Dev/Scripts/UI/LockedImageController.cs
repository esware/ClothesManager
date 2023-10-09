using System;
using EWGames.Dev.Scripts.Machines;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace EWGames.Dev.Scripts
{
    public class LockedImageController : MonoBehaviour
    { 
        private Image _lockedImage;
        private MachineBase _machineBase;
        private TextMeshProUGUI _lockedText;
        private Button _button;
        private Collider _collider;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _machineBase = GetComponentInParent<MachineBase>();
            _lockedText = GetComponentInChildren<TextMeshProUGUI>();
            _lockedImage = GetComponent<Image>();
            _collider = GetComponentInParent<BoxCollider>();
            
            SignUpEvents();
        }

        private void Start()
        {
            MachineStatusControl(GameManager.Instance.currentLevel);
        }

        void SignUpEvents()
        {
            ClothingItem.OnItemSold += MachineStatusControl;
        }

        void MachineStatusControl(int price)
        {
            if (_machineBase.machineData.unlockedPrice == 0 || !_machineBase.machineData.isLocked)
            {
                _machineBase.isLocked = false;
                _collider.enabled = true;
                _lockedImage.gameObject.SetActive(false);
                return;
            }

            _button.enabled = false;
            _collider.enabled = false;
            
            if (GameManager.Instance.currentLevel+1>= _machineBase.machineData.unlockedLevel)
            {
                if (GameManager.Instance.currentMoney >= _machineBase.machineData.unlockedPrice)
                {
                    _lockedImage.color=Color.green;
                    _button.enabled = true;
                }
                else
                {
                    _lockedText.text = _machineBase.machineData.unlockedPrice.ToString();
                    _lockedImage.color=Color.yellow;
                }
               
            }
            else
            {
                _lockedImage.color=Color.yellow;
                _lockedText.text = "LEVEL "+_machineBase.machineData.unlockedLevel.ToString();
            }

            
        }

        public void BuyMachine()
        {
            _collider.enabled = true;
            _lockedImage.gameObject.SetActive(false);
            _machineBase.machineData.isLocked = false;
            GameManager.Instance.BuyMachine(_machineBase.machineData.unlockedPrice);
        }
    }
}