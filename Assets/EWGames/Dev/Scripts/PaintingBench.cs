using System;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using EWGames.Dev.Scripts.Serialization;
using JetBrains.Annotations;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace EWGames.Dev.Scripts
{
    public class PaintingBench : MonoBehaviour
    {
        private  Dictionary<ClothingItem, Vector3> _items = new Dictionary<ClothingItem, Vector3>();

        public List<string> itemNameList = new List<string>();
        
        private void OnEnable()
        {
            SignUpEvents();
            LoadItems();
        }
        private void OnDisable()
        {
            SaveItems();
        }
        void SignUpEvents()
        {
            ClothingItem.OnReadyForPaint += GetReadyClothes;
        }

        #region save&load

        private void SaveItemNameList()
        {
            string saveNames = string.Join(",", itemNameList.ToArray());
            PlayerPrefs.SetString("SaveNames", saveNames);
        }
        private void LoadItemNameList()
        {
            if (PlayerPrefs.HasKey("SaveNames"))
            {
                string saveNames = PlayerPrefs.GetString("SaveNames");
                itemNameList = new List<string>(saveNames.Split(','));
            }
        }
        private void SaveItems()
        {
            itemNameList.Clear();
            foreach (var item in _items)
            {
                string itemName = item.Key.itemData.name;

                while (itemNameList.Contains(itemName))
                {
                    itemName += "0";
                }
                    
                ItemDataSerialization.SaveClothingItemData(itemName, item.Key.itemData);
                itemNameList.Add(itemName);
                
            }
            
            SaveItemNameList();
            
        }
        private void LoadItems()
        {
            _items.Clear();
            LoadItemNameList();
            
            foreach (string itemName in itemNameList)
            {
                string path = Application.persistentDataPath + "/saves/" + itemName + ".save";

                if (File.Exists(path))
                {
                    ClothingItemData loadedClothingItemData = ItemDataSerialization.LoadClothingItemData(path);

                    if (loadedClothingItemData != null && loadedClothingItemData.clothesModel!=null)
                    {
                        var item = Instantiate(loadedClothingItemData.clothesModel, transform);
                        item.GetComponent<ClothingItem>().itemData = loadedClothingItemData;
                        item.transform.localPosition = ObjectsToPlace(item.GetComponent<ClothingItem>());
                    }
                }
            }
        }

        #endregion
        
        private void GetReadyClothes(ClothingItem clothesController,string id)
        {
            clothesController.transform.SetParent(transform);
            Transform transform1;
            
            (transform1 = clothesController.transform).localPosition = ObjectsToPlace(clothesController);
            transform1.DOScale(Vector3.one, 0.3f);
            transform1.localRotation = Quaternion.identity;
        }
        private Vector3 ObjectsToPlace(ClothingItem c)
        {
            var defaultPosition=new Vector3(-0.3f, 0.2f, -0.2f);
            
            for (int i = 0; i < _items.Count; i++)
            {
                if (!_items.ContainsValue(defaultPosition))
                {
                    return defaultPosition;
                }
                defaultPosition.x += 0.3f;
            }
            _items.Add(c,defaultPosition);
            return defaultPosition;
        }
        public void RemovePositionFromList(ClothingItem c)
        {
            List<ClothingItem> clothes = new List<ClothingItem>();

            _items.Remove(c);
            foreach (var p in _items)
            {
                clothes.Add( p.Key);
            }
            _items.Clear();

            foreach (var p in clothes)
            {
                p.transform.DOLocalMove(ObjectsToPlace(p),1f);
            }
        }
    }
}