using System;
using System.Collections;
using DG.Tweening;
using EWGames.Dev.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PaintMachine : MonoBehaviour
{
    public static Action<Clothes> OnPaintFinish;

    [Space, Header("Machine Settings")]
    [SerializeField] private float paintTime;
    [SerializeField] private GameObject durationCanvas;
    [SerializeField] private Color color;
    public bool MachineIsRunning { get; private set; } = false;
    
    public float radius = 2f;
    private float angle = 0f;
    public float speed = 1f;

    public Image timer;


    private void Start()
    {
        SignUpEvents();
    }

    void SignUpEvents()
    {
        Clothes.OnClothesLocated += PaintStarted;
    }

    void PaintStarted(PaintMachine machine,Clothes clothes)
    {
        
        if (machine==this)
        {
            StartCoroutine(Paint(clothes));
        }
    }

    private IEnumerator Paint(Clothes clothes)
    {
        MachineIsRunning = true;
        
        timer.fillAmount = 0;
        durationCanvas.gameObject.SetActive(true);
        
        durationCanvas.SetActive(true);
        var circle = timer.GetComponent<Image>();  
        var percent = 1 / paintTime;
        var t = 0f;
        
        float startAngle = 0f;

        Material mat = clothes.GetComponent<Renderer>().materials[0];
        mat.DOColor(color, paintTime);
        

        while (t < 1f)
        {
            angle = startAngle + speed * t;

            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle) * 0.8f;

            clothes.transform.localPosition = new Vector3(x, 0.18f, y - 0.05f);
            t += percent*Time.deltaTime;
            circle.fillAmount = t;
            yield return null;
        }
        
        OnPaintFinish?.Invoke(clothes);
        durationCanvas.gameObject.SetActive(false);
        MachineIsRunning = false;
    }

}
