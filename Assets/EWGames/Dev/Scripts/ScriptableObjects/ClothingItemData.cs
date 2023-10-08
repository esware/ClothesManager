using UnityEngine;

namespace EWGames.Dev.Scripts
{
    [CreateAssetMenu(fileName = "",menuName = "EwGames/Data/ClothesData")]
    public class ClothingItemData:ScriptableObject
    {
        [SerializeField] public float price;
        [SerializeField] public GameObject clothesModel;
        [SerializeField] public Sprite itemSprite;

        [HideInInspector] public ColorCode color;

    }
}