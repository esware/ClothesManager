﻿using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EWGames.Dev.Scripts
{
    public class PaintButtonController : MonoBehaviour,IPointerClickHandler
    {
        private Camera _mainCam;
        private TextMeshProUGUI _buttonText;
        private Image _buttonImage;
        private void Awake()
        {
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
            _buttonImage = GetComponent<Image>();
            _mainCam=Camera.main;
        }

        public void InteractButton()
        {
            var defaultColor = _buttonImage.color;
            
            transform.DOScale(Vector3.one*1.2f, 0.2f).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.2f);
            });
            
            _buttonImage.DOColor(Color.blue*0.8f, 0.2f).OnComplete(() =>
            {
                _buttonImage.DOColor(defaultColor, 0.2f);
            });
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_mainCam.transform.position.x >1)
            {
                SewSection();
            }
            else
            {
                PaintSection();
            }
        }

        void SewSection()
        {
            _mainCam.transform.localPosition = new Vector3(0f, 1.1f, -1.6f);
            _buttonText.text = "PAINT";
        }

        void PaintSection()
        {
            _mainCam.transform.localPosition = new Vector3(1.5f, 1.1f, -1.6f);
            _buttonText.text = "SEW";
        }
    }
}