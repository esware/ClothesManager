using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EWGames.Dev.Scripts;
using EWGames.Dev.Scripts.Machines;
using EWGames.Dev.Scripts.Missions;
using EWGames.Dev.Scripts.ShopItem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PaintMachine : MachineBase
{
    #region Inspector Variables

    [Space, Header("Machine Settings")]
    [SerializeField] private GameObject durationCanvas;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Image timer;
    

    #endregion
    
    public bool MachineIsRunning { get; private set; }

    private void Start()
    {
        SignUpEvents();
    }
    private void SignUpEvents()
    {
        ClothingItem.OnClothesLocated += PaintStarted;
    }
    private void PaintStarted(PaintMachine machine,ClothingItem clothes)
    {
        if (machine==this)
        {
            StartCoroutine(Paint(clothes));
        }
    }
    private void InitializeTimer()
    {
        timer.fillAmount = 0;
        timer.transform.parent.gameObject.SetActive(true);
    }
    private void UpdateItemPosition(ClothingItem item,float elapsedTime)
    {
        float time =  speed * elapsedTime;

        float x = radius * Mathf.Cos(time);
        float y = radius * Mathf.Sin(time) * 0.8f;

        item.transform.localPosition = new Vector3(x, 0.18f, y - 0.05f);
    }
    private void UpdateTimer(float elapsedTime, float duration)
    {
        timer.fillAmount = elapsedTime / duration;
    }
    private void FinishPaint(ClothingItem item)
    {
        item.itemData.color = machineData.color;
        item.price = item.itemData.price + machineData.earnedMoney;

        item.MoveToStartPosition(durationCanvas.transform);

        timer.transform.parent.gameObject.SetActive(false);
        MachineIsRunning = false;
    }
    private IEnumerator Paint(ClothingItem item)
    {
        MachineIsRunning = true;
        InitializeTimer();

        item.PaintItem(ColorMapper.GetColorFromCode(machineData.color), machineData.operationTime);

        float duration = machineData.operationTime;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            UpdateItemPosition(item,elapsedTime);
            UpdateTimer(elapsedTime, duration);

            elapsedTime += Time.deltaTime ;
            yield return null;
        }

        FinishPaint(item);
    }

}
