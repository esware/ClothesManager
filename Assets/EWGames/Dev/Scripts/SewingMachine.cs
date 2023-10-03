using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EWGames.Dev.Scripts;
using UnityEngine;

public class SewingMachine : MonoBehaviour
{
    [SerializeField] public ClothesData clothesData;
    public GameObject needle;
    public GameObject rope;
    public List<GameObject> ropes;
    public SpriteRenderer clothesRenderer;
    public ColorChange ColorChange;

    private void Start()
    {
        foreach (Transform r in rope.transform)
        {
            ropes.Add(r.gameObject);
        }

        clothesRenderer.sprite = clothesData.clothesImage;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(Sew());
        }
    }

    IEnumerator Sew()
    {
        var duration = clothesData.sewingTime/ropes.Count;

        var animator = clothesRenderer.gameObject.GetComponent<Animator>();
        animator.speed = 1/clothesData.sewingTime;
        animator.Play("ClothSew",0);


        foreach (var r in ropes)
        {
            r.transform.DOScale(new Vector3(0, 1, 0), duration).SetEase(Ease.InSine);
            needle.transform.DOMoveY(0.06f, duration / 2).OnComplete(() =>
            {
                needle.transform.DOMoveY(0.12f, duration / 2);
            });
            yield return new WaitForSeconds(duration);
        }
        
        ColorChange.ChangeColor();
    }
}
