using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using EWGames.Dev.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SewingMachine : MonoBehaviour
{
    [Space,Header("Particles")]
    [SerializeField] private ParticleSystem finishSparkParticle;
    [SerializeField] private ParticleSystem finishCloudParticle;
    
    [Space,Header("Machine Components")]
    [SerializeField] private ClothesData clothesData;
    [SerializeField] private GameObject needle;
    [SerializeField] private GameObject ropeObject;
    [SerializeField] private SpriteRenderer sewingClothes;
    
    [Space, Header("Canvas")] 
    [SerializeField] private RectTransform mainCanvas;

    public bool MachineIsRunning { get; private set; } = false;

    private ColorTransition _colorTransition;
    private Coroutine _needleAnimationCoroutine;

    private void Start()
    {
        InitialComponents();
        SignUpEvents();
    }

    private void InitialComponents()
    {
        sewingClothes.sprite = clothesData.clothesImage;

        _colorTransition = GetComponentInChildren<ColorTransition>();
        
        _colorTransition.SetIcons(clothesData.clothesImage);
    }

    private void SignUpEvents()
    {
        RopeControl.OnRopeLocated += ValidateSew;
        ClothesUI.OnReadyForPaint += MachineReady;
    }

    private void MachineReady(ClothesUI arg1, string id)
    {
        if (id==name)
        {
            MachineIsRunning = false;
            _colorTransition.ResetColor();
        }
    }


    private void ValidateSew(SewingMachine machine)
    {
        if (machine==this && !MachineIsRunning)
        {
            Func<Task> func = Sew;
            func();
        }
    }

    private async Task Sew()
    {
        MachineIsRunning = true;
        SetRopeSlot(true);
    
        var duration = clothesData.sewingTime / GetRopeChild().Count;
        var percent = 2.5f / GetRopeChild().Count;

        _needleAnimationCoroutine = StartCoroutine(AnimateNeedle());

        foreach (var r in GetRopeChild())
        {
            r.transform.DOScale(new Vector3(0, 1, 0), duration).SetEase(Ease.InSine);
            sewingClothes.transform.DOLocalMoveX(sewingClothes.transform.localPosition.x+percent, duration);
            await Task.Delay((int)(duration * 1000));
        }
        
        if (_needleAnimationCoroutine != null)
        {
            StopCoroutine(_needleAnimationCoroutine);
        }

        await PlayFinishParticle();
        _colorTransition.ChangeColor();
        InstantiateClothes();
    }
    private async Task PlayFinishParticle()
    {
        finishSparkParticle.Play();
        await Task.Delay(400);
        finishCloudParticle.Play();
    }
    private IEnumerator AnimateNeedle()
    {
        while (true)
        {
            yield return needle.transform.DOLocalMoveY(0.09f, 0.2f).WaitForCompletion();
            yield return needle.transform.DOLocalMoveY(0.14f, 0.2f).WaitForCompletion();
        }
    }
    
    private void SetRopeSlot(bool isActive)
    {
        foreach (Transform t in ropeObject.transform)
        {
            t.gameObject.SetActive(isActive);
            t.transform.localScale = isActive? Vector3.one : Vector3.zero;
        }
    }

    private List<GameObject> GetRopeChild()
    {
        List<GameObject> ropeLoops = new List<GameObject>();
        foreach (Transform t in ropeObject.transform)
        {
            ropeLoops.Add(t.gameObject);
        }

        return ropeLoops;
    }

    private void InstantiateClothes()
    {
        var newClothes = Instantiate(clothesData.clothesModel,mainCanvas.transform);
        newClothes.gameObject.name = gameObject.name;
        sewingClothes.transform.localPosition=Vector3.zero;

        newClothes.transform.localScale=Vector3.zero;
        
        newClothes.transform.DOScale(Vector3.one * 0.03f, 1f);
        newClothes.transform.DORotate(Vector3.zero, 1f);
        newClothes.transform.DOLocalMove(new Vector3(0, 2f, -3f), 1f);
    }




}
