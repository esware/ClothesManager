using EWGames.Dev.Scripts;
using EWGames.Dev.Scripts.Serialization;
using UnityEditor;
using UnityEngine;

[System.Serializable]
    public class SerializableClothingItemData
    {
        public int price;
        public string clothesModelPrefabPath;
        public string itemSpritePath;
        public ColorCode color;
    }

    public static class ItemDataSerialization
    {
        private static SerializableClothingItemData ConvertToSerializable(ClothingItemData original)
        {
            SerializableClothingItemData serializableData = new SerializableClothingItemData
            {
                price = original.price,
#if UNITY_EDITOR
                clothesModelPrefabPath = AssetDatabase.GetAssetPath(original.clothesModel),
                itemSpritePath = AssetDatabase.GetAssetPath(original.itemSprite),
#endif
                color = original.color
            };

            return serializableData;
        }

        private static ClothingItemData ConvertFromSerializable(SerializableClothingItemData serializableData)
        {
            ClothingItemData original = ScriptableObject.CreateInstance<ClothingItemData>();
            original.price = serializableData.price;
#if UNITY_EDITOR
            original.clothesModel = AssetDatabase.LoadAssetAtPath<GameObject>(serializableData.clothesModelPrefabPath);
            original.itemSprite = AssetDatabase.LoadAssetAtPath<Sprite>(serializableData.itemSpritePath);
#endif
            original.color = serializableData.color;

            return original;
        }

        public static void SaveClothingItemData(string saveName, ClothingItemData clothingItemData)
        {
            SerializableClothingItemData serializableData = ConvertToSerializable(clothingItemData);
            SerializationManager.Save(saveName, serializableData);
        }
        public static ClothingItemData LoadClothingItemData(string path)
        {
            SerializableClothingItemData serializableData = SerializationManager.Load(path) as SerializableClothingItemData;

            if (serializableData != null)
            {
                return ConvertFromSerializable(serializableData);
            }

            return null;
        }
        
    }