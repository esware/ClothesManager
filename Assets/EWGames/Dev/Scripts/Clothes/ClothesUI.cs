using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EWGames.Dev.Scripts
{
    public class ClothesUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static Action<ClothesUI,string> OnReadyForPaint;

        #region Private Variables
        
        private Transform _canvas;

        #endregion


        private void Start()
        {
            _canvas = GameObject.Find("---UI---").transform;
        }

        #region Draging Methods

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_canvas);
        }
        public void OnDrag(PointerEventData eventData)
        {
            var transform1 = transform;
            transform1.localScale=Vector3.one *3f;
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
                    button.GetComponent<PaintButtonController>().InteractButton();
                    transform.DOScale(Vector3.zero, 0.3f);
                    OnReadyForPaint?.Invoke(this,name);
                    break;
                }
            }
        }

        #endregion

       
    }
}