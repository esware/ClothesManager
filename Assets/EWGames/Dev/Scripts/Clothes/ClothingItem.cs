using System;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using EWGames.Dev.Scripts.ShopItem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace EWGames.Dev.Scripts
{
    public class ClothingItem : MonoBehaviour,IPointerClickHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static Action<ClothingItem, string> OnReadyForPaint;
        public static Action<PaintMachine,ClothingItem> OnClothesLocated;
        public static Action<int> OnItemSold;

        public ClothingItemData itemData;
        public int price;
        public string targetTag;

        #region Private Variables

        private const string CanvasObjectName = "---UI---";
        private Transform _canvas;
        private bool _isSelected;
        private Transform _targetTransform;
        private Camera _mainCam;
        private Vector3 _mousePosition;
        private Transform _parent;
        private Vector3 _startPosition;
        private bool _isPaintFinish;
        private UIManager _uiManager;
        private Image _clothImage;

        #endregion

        private void Start()
        {
            _canvas = GameObject.Find(CanvasObjectName).transform;
            _mainCam=Camera.main;
            _uiManager = FindObjectOfType<UIManager>();
            _clothImage = GetComponent<Image>();
        }

        #region Input Class

        #region UI Input
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isPaintFinish)
            {
                transform.SetParent(_canvas);
                transform.DOLocalMove(Vector3.zero, .1f);

                transform.DOScale(Vector3.one * 3, 1f).OnComplete(() =>
                {
                    var image = GetComponent<Image>();
                    image.color = Color.white;
                    image.sprite = _uiManager.moneyImage.sprite;

                    transform.DOMove(_uiManager.moneyImage.transform.position,
                        1f).OnUpdate(() =>
                    {
                        transform.DOScale(Vector3.one, 1f);
                    }).OnComplete(() =>
                    {
                        Shop.Instance.SellItem(itemData);
                        _uiManager.UpdateMission();
                        OnItemSold?.Invoke(price);
                        Destroy(gameObject);
                    });
                });
                
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_canvas);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var transform1 = transform;
            transform1.localScale = Vector3.one * 3f;
            transform1.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                Button button = result.gameObject.GetComponent<Button>();
                
                if (button != null)
                {
                    button.GetComponent<SectionSwitchController>().InteractButton();
                    transform.DOScale(Vector3.zero, 0.3f);
                    OnReadyForPaint?.Invoke(this, name);
                    
                    break;
                }
            }
        }

        #endregion

        #region Input

        private void OnMouseDown()
        {
            _isSelected = true;
            ObjectSelection();
        }

        private void OnMouseUp()
        {
            _isSelected = false;
            CanLocate(_targetTransform);
            
        }

        #endregion
        
        #endregion

        void Update()
        {
            if (_isSelected)
            {
                _mousePosition = Input.mousePosition;
                Vector3 worldPoint = _mainCam.ScreenToWorldPoint(_mousePosition);
                worldPoint.z = -0.2f;

                transform.position = new Vector3(worldPoint.x, worldPoint.y - 0.2f, worldPoint.z);
            }
        }

        public void PaintItem(Color color,float paintTime)
        {
            Material mat = GetComponentInChildren<Renderer>().materials[0];
            mat.DOColor(color, paintTime);

            _clothImage.DOColor(color, paintTime);
        }
        public void MoveToStartPosition(Transform canvas)
        {
            Transform t;
            (t = transform).SetParent(canvas);
            t.localScale=Vector3.zero;
            t.DOLocalMove(new Vector3(0,2,0), 1f);
            t.DOScale(Vector3.one*0.03f, 1f);
            _isPaintFinish = true;
        }
        private void CanLocate(Transform targetTransform)
        {
            if (!targetTransform)
            {
                ResetPosition();
            }
            else
            {
                if (targetTransform.GetComponent<PaintMachine>().MachineIsRunning)
                {
                   ResetPosition();
                }
                else
                {
                    Locate(targetTransform);   
                }
            }
        }
        private void Locate(Transform targetTransform)
        {
            _parent.GetComponent<PaintingBench>().RemovePositionFromList(this);
            transform.SetParent(targetTransform);
            transform.DOScale(Vector3.one*0.5f, 1f);
            transform.DOLocalMove(new Vector3(0.124715351f, 0.310062677f, -0.052471254f), 0.2f);

            var machine = targetTransform.GetComponent<PaintMachine>();
            OnClothesLocated?.Invoke(machine,this);
        }
        private void ResetPosition()
        {
            transform.SetParent(_parent);
            transform.DOLocalMove(_startPosition, 0.2f);
        }
        private void ObjectSelection()
        {
            _startPosition = transform.localPosition;
            _parent = transform.parent;
            SelectionEffect();
        }
        private void SelectionEffect()
        {
            transform.DOScale(Vector3.one * 0.8f, 0.05f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                transform.DOScale(Vector3.one * 0.7f, 0.2f);
            });
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
            if (other.CompareTag(targetTag))
            {
                _targetTransform = other.transform;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                _targetTransform = null;
            }
        }
    }
}