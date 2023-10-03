using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EWGames.Dev.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SewingMachine : MonoBehaviour
{
    [SerializeField] public ClothesData clothesData;
    public GameObject needle;
    public GameObject ropeObject;
    
    
    public SpriteRenderer clothesRenderer;
    public SpriteRenderer clothesIconRenderer;
    public ColorChange colorChange;

    private void Start()
    {
        clothesRenderer.sprite = clothesData.clothesImage;
        clothesIconRenderer.sprite = clothesData.clothesImage;
        SignUpEvents();
    }

    void SignUpEvents()
    {
        DragAndDrop.OnRopeLocated += SewStarted;
    }

    void SewStarted(SewingMachine machine)
    {
        if (machine==this)
        {
            StartCoroutine(Sew());
        }
    }
    
    IEnumerator Sew()
    {
        List<GameObject> ropeLoops = new List<GameObject>();
        foreach (Transform t in ropeObject.transform)
        {
            ropeLoops.Add(t.gameObject);
            t.gameObject.SetActive(true);
        }
        
        
        var duration = clothesData.sewingTime/ropeLoops.Count;

        var animator = clothesRenderer.gameObject.GetComponent<Animator>();
        animator.speed = 1/clothesData.sewingTime;
        animator.Play("ClothSew",0);


        foreach (var r in ropeLoops)
        {
            r.transform.DOScale(new Vector3(0, 1, 0), duration).SetEase(Ease.InSine);
            
            // Clothes sewing at machine
            needle.transform.DOLocalMoveY(0.09f, duration / 2).OnComplete(() =>
            {
                needle.transform.DOLocalMoveY(0.14f, duration / 2);
            });
            yield return new WaitForSeconds(duration);
        }
        
        foreach (Transform t in ropeObject.transform)
        {
            t.gameObject.SetActive(false);
            t.transform.localScale=Vector3.one;
        }
        
        colorChange.ChangeColor();
    }
    
}
