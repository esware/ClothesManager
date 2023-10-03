using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    public static Action<SewingMachine> OnRopeLocated;
    public Vector3 startPosition;
    
    private bool _isSelected = false;
    private bool _isDragging = false;
    private Transform _targetTransform;

    private Camera _mainCam;
    private Vector3 _mousePosition;
    private SewingMachine _sewingMachine;

    private void Start()
    {
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        _isSelected = true;
        
        SelectionEffect();
    }

    void OnMouseUp()
    {
        _isSelected = false;
        _isDragging = false;
        Locate();
    }

    void Update()
    {
        if (_isSelected)
        {
            _mousePosition  = Input.mousePosition;
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(_mousePosition);
            worldPoint.z = -0.1f;

            if (Vector3.Distance(startPosition, worldPoint) > 0.3f)
            {
                if(!_isDragging)
                {
                    _isDragging = true;
                    DraggingEffect();
                }
            }

            if (_isDragging)
            {
                transform.position = new Vector3(worldPoint.x,worldPoint.y-0.2f,worldPoint.z);
            }
        }
    }

    void Locate()
    {
        if (!_targetTransform)
        {
            transform.SetParent(null);
            transform.DOLocalMove(startPosition, 0.2f);
            transform.DOScale(Vector3.one, 0.2f);
        }
        else
        {
            transform.SetParent(_targetTransform);
            transform.DOLocalMove(new Vector3(0.124715351f, 0.310062677f, -0.052471254f), 0.2f);
            transform.DOScale(Vector3.one, 0.2f);
            
            OnRopeLocated?.Invoke(_sewingMachine);
            ObjectPool.Instance.ReturnObjectToPool(gameObject);
        }
        
    }

    void SelectionEffect()
    {
        transform.DOScale(Vector3.one * 1.5f, 0.05f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DOScale(Vector3.one* 1.2f, 0.2f);
        });
    }
    
    void DraggingEffect()
    {
        transform.DORotate(new Vector3(0,0,10), 0.05f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0,0,-10), 0.1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                transform.DORotate(Vector3.zero, 0.05f);
            });
        });
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SewingMachine"))
        {
            _targetTransform = other.transform;
            _sewingMachine = other.GetComponent<SewingMachine>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SewingMachine"))
        {
            _targetTransform = null;
            _sewingMachine = null;
        }
    }
    
}