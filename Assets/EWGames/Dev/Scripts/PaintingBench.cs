using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace EWGames.Dev.Scripts
{
    public class PaintingBench : MonoBehaviour
    {
        private readonly Dictionary<ClothingItem, Vector3> _positions = new Dictionary<ClothingItem, Vector3>();
        
        private void OnEnable()
        {
            SignUpEvents();
        }
        
        void SignUpEvents()
        {
            ClothingItem.OnReadyForPaint += GetReadyClothes;
        }

        void GetReadyClothes(ClothingItem clothesController,string id)
        {
            clothesController.transform.SetParent(transform);
            Transform transform1;
            
            (transform1 = clothesController.transform).localPosition = ObjectsToPlace(clothesController);
            transform1.DOScale(Vector3.one, 0.3f);
            transform1.localRotation = Quaternion.identity;

        }

        Vector3 ObjectsToPlace(ClothingItem c)
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

        public void RemovePositionFromList(ClothingItem c)
        {
            List<ClothingItem> clothes = new List<ClothingItem>();
            
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