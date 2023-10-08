using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EWGames.Dev.Scripts.Missions
{
    public class MissionUI : MonoBehaviour
    {
        public Image missionImage;
        public TextMeshProUGUI amountText;


        public void Initialize(Sprite missionSprite,Color itemColor,int amount)
        {
            amountText.text ="x"+ amount;
            missionImage.sprite = missionSprite;
            missionImage.color = itemColor;
        }

        public void UpdateCountText(int amount)
        {
            amountText.text ="x"+ amount;
        }
    }
}