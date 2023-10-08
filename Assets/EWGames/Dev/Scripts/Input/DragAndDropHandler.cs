using System;
using UnityEngine;

namespace EWGames.Dev.Scripts
{
    public class DragAndDropHandler : MonoBehaviour
    {
        public static Action<GameObject> OnDragStart;
        public static Action<Transform ,GameObject> OnDragEnd;

        public string targetTag;
        
       
        private Transform _targetTransform;
        private Camera _mainCam;
        private Vector3 _mousePosition;
        private bool _isSelected;


        private void Awake()
        {
            _mainCam=Camera.main;
        }

        private void OnMouseDown()
        {
            _isSelected = true;
            OnDragStart?.Invoke(gameObject);
        }

        private void OnMouseUp()
        {
            _isSelected = false;

            OnDragEnd?.Invoke(_targetTransform,gameObject);
        }


        void Update()
        {
            if (_isSelected)
            {
                _mousePosition  = Input.mousePosition;
                Vector3 worldPoint = _mainCam.ScreenToWorldPoint(_mousePosition);
                worldPoint.z = -0.2f;

                transform.position = new Vector3(worldPoint.x,worldPoint.y-0.2f,worldPoint.z);
            }
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