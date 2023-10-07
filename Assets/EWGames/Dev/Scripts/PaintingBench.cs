using System;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace EWGames.Dev.Scripts
{
    public class PaintingBench : MonoBehaviour
    {
        private readonly Dictionary<Clothes, Vector3> _positions = new Dictionary<Clothes, Vector3>();
        private void OnEnable()
        {
            SignUpEvents();
        }
        void SignUpEvents()
        {
            ClothesUI.OnReadyForPaint += GetReadyClothes;
        }

        void GetReadyClothes(ClothesUI clothesController,string id)
        {
            var child = clothesController.transform.GetChild(0).gameObject;
            child.gameObject.SetActive(true);

            child.transform.SetParent(transform);
            child.transform.localPosition = ObjectsToPlace(child.GetComponent<Clothes>());
            child.transform.localScale =Vector3.one*0.7f;
            child.transform.localRotation = Quaternion.identity;

            clothesController.transform.SetParent(child.transform);

        }

        Vector3 ObjectsToPlace(Clothes c)
        {
            var defaultPosition=new Vector3(-0.3f, 0.2f, -0.2f);
            
            for (int i = 0; i < _positions.Count; i++)
            {
                if (!_positions.ContainsValue(defaultPosition))
                {
                    return defaultPosition;
                }
                defaultPosition.x += 0.3f;
            }
            _positions.Add(c,defaultPosition);
            
            return defaultPosition;
        }

        public void RemovePositionFromList(Clothes c)
        {
            List<Clothes> clothes = new List<Clothes>();
            
            _positions.Remove(c);
            foreach (var p in _positions)
            {
                clothes.Add( p.Key);
            }
            _positions.Clear();

            foreach (var p in clothes)
            {
                p.transform.DOLocalMove(ObjectsToPlace(p),1f);
            }
        }
    }
}