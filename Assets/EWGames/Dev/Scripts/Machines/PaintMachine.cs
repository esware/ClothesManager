using System;
using System.Collections;
using DG.Tweening;
using EWGames.Dev.Scripts;
using EWGames.Dev.Scripts.Missions;
using EWGames.Dev.Scripts.ShopItem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PaintMachine : MonoBehaviour
{

    [Space, Header("Machine Settings")] 
    [SerializeField] private MachineData machineData;
    [SerializeField] private GameObject durationCanvas;
    
    [Space,Header("Machine Locked Settings")]
    [SerializeField]
    private Image lockedImage;
    public bool MachineIsRunning { get; private set; } = false;
    
    
    public float radius = 2f;
    private float angle = 0f;
    public float speed = 1f;
    public Image timer;
    
    private bool _isMachineLocked = true;

    private void Start()
    {
        SignUpEvents();
        InitMachine();
        
        if (_isMachineLocked)
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    void SignUpEvents()
    {
        ClothingItem.OnClothesLocated += PaintStarted;
    }

    void PaintStarted(PaintMachine machine,ClothingItem clothes)
    {
        
        if (machine==this)
        {
            StartCoroutine(Paint(clothes));
        }
    }

    private IEnumerator Paint(ClothingItem item)
    {
        MachineIsRunning = true;
        timer.fillAmount = 0;
        timer.transform.parent.gameObject.SetActive(true);
        
        var circle = timer.GetComponent<Image>();  
        var percent = 1 / machineData.operationTime;
        var t = 0f;
        
        float startAngle = 0f;

        item.PaintItem(ColorMapper.GetColorFromCode(machineData.color),machineData.operationTime);
        

        while (t < 1f)
        {
            angle = startAngle + speed * t;

            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle) * 0.8f;

            item.transform.localPosition = new Vector3(x, 0.18f, y - 0.05f);
            t += percent*Time.deltaTime;
            circle.fillAmount = t;
            yield return null;
        }

        item.itemData.color = machineData.color;
        item.itemData.price += machineData.earnedMoney;
        
        Shop.Instance.SellItem(item.itemData);
        
        item.MoveToStartPosition(durationCanvas.transform);
        
        timer.transform.parent.gameObject.SetActive(false);
        MachineIsRunning = false;
    }
    
    private void InitMachine()
    {
        var level =PlayerPrefs.GetInt("CurrentLevel");
        
        if (machineData.unlockedPrice==0)
        {
            _isMachineLocked = false;
            lockedImage.gameObject.SetActive(false);
            return;
        }

        if (level>= machineData.unlockedLevel)
        {
            lockedImage.GetComponentInChildren<TextMeshProUGUI>().text = machineData.unlockedPrice.ToString();
        }
        else
        {
            lockedImage.GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL "+machineData.unlockedLevel.ToString();
        }
    }

}
