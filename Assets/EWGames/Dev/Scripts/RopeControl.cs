using System;
using DG.Tweening;
using EWGames.Dev.Scripts;
using UnityEngine;

public class RopeControl : MonoBehaviour
{
    public static Action<SewingMachine> OnRopeLocated;
    private Transform _parent;
    private Vector3 _startPosition;

    private void Start()
    {
        _parent = transform.parent;
    }

    private void OnEnable()
    {
        DragAndDropHandler.OnDragEnd += Locate;
        DragAndDropHandler.OnDragStart += SelectionEffect;
    }

    private void OnDisable()
    {
        DragAndDropHandler.OnDragEnd -= Locate;
        DragAndDropHandler.OnDragStart -= SelectionEffect;
    }

    void Locate(Transform targetTransform,GameObject id)
    {
        if (id!=gameObject)
            return;
        
        if (!targetTransform)
        {
            transform.SetParent(_parent);
            transform.DOLocalMove(_startPosition, 0.2f);
        }
        else
        {
            if (!targetTransform.GetComponent<SewingMachine>().MachineIsRunning)
            {
                _parent.GetComponent<RopeSpawn>().RemovePositionFromList(this);
                transform.SetParent(targetTransform);
                transform.DOLocalMove(new Vector3(0.124715351f, 0.310062677f, -0.052471254f), 0.2f);
                transform.DOScale(Vector3.one, 0.2f);
                OnRopeLocated?.Invoke(targetTransform.GetComponent<SewingMachine>());
                gameObject.SetActive(false);
                
            }
            else
            {
                transform.SetParent(_parent);
                transform.DOLocalMove(_startPosition, 0.2f);
            }
           

        }
        
    }

    void SelectionEffect(GameObject g)
    {
        _parent = transform.parent;
        _startPosition = transform.localPosition;
        
        if (g!=gameObject)
            return;
        
        transform.DOScale(Vector3.one * 1.2f, 0.05f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DOScale(Vector3.one* 1f, 0.2f);
        });
        transform.DORotate(new Vector3(0,0,10), 0.05f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0,0,-10), 0.1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                transform.DORotate(Vector3.zero, 0.05f);
            });
        });
    }
    
}
