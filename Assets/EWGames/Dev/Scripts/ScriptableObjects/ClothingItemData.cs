using UnityEngine;

namespace EWGames.Dev.Scripts
{
    [CreateAssetMenu(fileName = "",menuName = "EwGames/Data/ClothesData")]
    [System.Serializable]
    public class ClothingItemData:ScriptableObject
    {
        [SerializeField] public int price;
        [SerializeField] public GameObject clothesModel;
        [SerializeField] public Sprite itemSprite;

        [HideInInspector] public ColorCode color;

    }
}