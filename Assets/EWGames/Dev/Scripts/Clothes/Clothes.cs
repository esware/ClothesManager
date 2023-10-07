using System;
using System.Collections;
using System.Numerics;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace EWGames.Dev.Scripts
{
    public class Clothes : MonoBehaviour
    {
        public static Action<PaintMachine,Clothes> OnClothesLocated;
        public Sprite money;
        public Transform parent;
        
        private Transform _canvas;
        private Vector3 _startPosition;
        private void Awake()
        {
            _canvas = GameObject.Find("---UI---").transform;
        }

        private void Start()
        {
            parent = transform.parent;
        }

            private void OnEnable()
            {
                DragAndDropHandler.OnDragEnd += Locate;
                DragAndDropHandler.OnDragStart += SelectionEffect;
                PaintMachine.OnPaintFinish += PaintFinish;
            }
        
            private void OnDisable()
            {
                DragAndDropHandler.OnDragEnd -= Locate;
                DragAndDropHandler.OnDragStart -= SelectionEffect;
                PaintMachine.OnPaintFinish -= PaintFinish;
            }

            private void PaintFinish(Clothes obj)
            {
                if (obj==this)
                {
                    StartCoroutine(FinishPaint());
                }
            }

            private IEnumerator FinishPaint()
            {
                transform.DOLocalMove(new Vector3(0, 0.5f, -0.5f),1f);
                
                yield return new WaitForSeconds(1f);
                var child = transform.GetChild(0).gameObject;
                child.gameObject.GetComponent<Image>().color = GetComponent<Renderer>().materials[0].color;
                child.gameObject.SetActive(true);
                child.transform.SetParent(_canvas);
                child.transform.DOLocalMove(Vector3.zero, 0.1f);
                child.transform.DOScale(Vector3.one*5, .5f);
                transform.SetParent(child.transform);
                yield return new WaitForSeconds(1f);
                var s=child.GetComponent<Image>();
                s.sprite=money;
                s.color = Color.white;
                AnimateMoney(child.gameObject);
            }
            
            void AnimateMoney(GameObject child)
            {
                child.transform.DOMove(_canvas.GetComponent<UIManager>().moneyImage.rectTransform.position, 1f).OnComplete(
                    () =>
                    {
                        child.transform.gameObject.SetActive(false);
                    });
                child.transform.DOScale(Vector3.zero, 1f);
            }

            void Locate(Transform targetTransform,GameObject id)
            {
                if (id!=gameObject)
                    return;
                
                if (!targetTransform)
                {
                    transform.SetParent(parent);
                    transform.DOLocalMove(_startPosition, 0.2f);
                }
                else
                {
                    if (targetTransform.GetComponent<PaintMachine>().MachineIsRunning)
                    {
                        transform.SetParent(parent);
                        transform.DOLocalMove(_startPosition, 0.2f);
                    }
                    else
                    {
                        parent.GetComponent<PaintingBench>().RemovePositionFromList(this);
                        transform.SetParent(targetTransform);
                        transform.DOLocalMove(new Vector3(0.124715351f, 0.310062677f, -0.052471254f), 0.2f);

                        var machine = targetTransform.GetComponent<PaintMachine>();
                        OnClothesLocated?.Invoke(machine,this);
                    }
                }
            }
        
            void SelectionEffect(GameObject g)
            {
                parent = transform.parent;
                _startPosition = transform.localPosition;
                
                if (g!=gameObject)
                    return;
                
                transform.DOScale(Vector3.one * 0.8f, 0.05f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    transform.DOScale(Vector3.one* 0.7f, 0.2f);
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
}