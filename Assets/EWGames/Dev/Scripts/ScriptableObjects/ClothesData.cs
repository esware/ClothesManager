using UnityEngine;

namespace EWGames.Dev.Scripts
{
    [CreateAssetMenu(fileName = "",menuName = "EwGames/Data/ClothesData")]
    public class ClothesData:ScriptableObject
    {
        [SerializeField] public float sewingTime;
        [SerializeField] public float price;
        [SerializeField] public float paintingTime;
        [SerializeField] public GameObject clothesModel;
        [SerializeField] public Sprite clothesImage;

    }
}