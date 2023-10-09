using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using EWGames.Dev.Scripts;
using EWGames.Dev.Scripts.Machines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SewingMachine : MachineBase
{
    [Space,Header("Particles")]
    [SerializeField] private ParticleSystem sparkParticleEffect;
    [SerializeField] private ParticleSystem finishCloudParticle;

    [Space, Header("Machine Components")]
    [SerializeField] private ClothingItemData clothingItemData;
    [SerializeField] private GameObject needle;
    [SerializeField] private GameObject ropeObject;
    [SerializeField] private SpriteRenderer sewingClothes;
    

    [Space, Header("Canvas")] 
    [SerializeField] private RectTransform mainCanvas;

    public bool MachineIsRunning { get; private set; } = false;

    private ColorTransition _colorTransition;

    private void Start()
    {
        InitialComponents();
        SignUpEvents();
    }

    private void InitialComponents()
    {
        sewingClothes.sprite = clothingItemData.itemSprite;

        _colorTransition = GetComponentInChildren<ColorTransition>();
        
        _colorTransition.SetIcons(clothingItemData.itemSprite);
    }
    private void SignUpEvents()
    {
        RopeControl.OnRopeLocated += AcceptNewThreadIfMachineIdle;
        ClothingItem.OnReadyForPaint += OnMachineReadyForClothingItem;
    }

    #region Machine State Control
    private void OnMachineReadyForClothingItem(ClothingItem item, string id)
    {
        if (id==name)
        {
            MachineIsRunning = false;
            _colorTransition.ResetColor();
            StopFinishParticles();
        }
    }
    private void AcceptNewThreadIfMachineIdle(SewingMachine machine)
    {
        if (machine == this && !MachineIsRunning)
        {
            Func<Task> func = StartSewingProcess;
            func();
        }
    }
    private async Task StartSewingProcess()
    {
        MachineIsRunning = true;
        SetRopeSlotsVisibility();
    
        var duration = machineData.operationTime / GetActiveRopeLoops().Count;
        var percent = 2.5f / GetActiveRopeLoops().Count;
        
        foreach (var r in GetActiveRopeLoops())
        {
            r.transform.DOScale(new Vector3(0, 1, 0), duration).SetEase(Ease.InSine);
            sewingClothes.transform.DOLocalMoveX(sewingClothes.transform.localPosition.x+percent, duration);
            await AnimateNeedle();
            await Task.Delay((int)(duration * 1000));
        }
        
        await PlayFinishParticles();
        _colorTransition.ChangeColor();
        InstantiateClothes();
    }

    #endregion
    
    #region Particle Play & Stop

    private async Task PlayFinishParticles()
    {
        sparkParticleEffect.Play();
        await Task.Delay(400);
        finishCloudParticle.Play();
    }
    private void StopFinishParticles()
    {
        sparkParticleEffect.Stop();
        finishCloudParticle.Stop();
    }

    #endregion
    
    #region Machine Methods

    private async Task AnimateNeedle()
    {
        needle.transform.DOLocalMoveY(0.09f, 0.1f).OnComplete(() =>
        {
            needle.transform.DOLocalMoveY(0.13f, 0.1f).WaitForCompletion();
        });
        
        await Task.CompletedTask;
    }
    private void SetRopeSlotsVisibility()
    {
        foreach (Transform t in ropeObject.transform)
        {
            t.gameObject.SetActive(true);
            t.transform.localScale =  Vector3.one;
        }
    }
    private List<GameObject> GetActiveRopeLoops()
    {
        List<GameObject> ropeLoops = new List<GameObject>();
        foreach (Transform t in ropeObject.transform)
        {
            ropeLoops.Add(t.gameObject);
        }

        return ropeLoops;
    }

    #endregion

    private void InstantiateClothes()
    {
        var newClothes = Instantiate(clothingItemData.clothesModel,mainCanvas.transform);
        
        var item = newClothes.GetComponent<ClothingItem>();
        item.price = item.itemData.price + machineData.earnedMoney;
        
        newClothes.gameObject.name = gameObject.name;
        sewingClothes.transform.localPosition=Vector3.zero;
        newClothes.transform.localScale=Vector3.zero;
        newClothes.transform.DOScale(Vector3.one * 0.03f, 1f);
        newClothes.transform.DORotate(Vector3.zero, 1f);
        newClothes.transform.DOLocalMove(new Vector3(0, 2f, -3f), 1f);
    }




}
