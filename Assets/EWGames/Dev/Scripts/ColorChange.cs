using DG.Tweening;
using UnityEngine;

namespace EWGames.Dev.Scripts
{
    public class ColorChange : MonoBehaviour
    {
        public SpriteRenderer[] spriteRenderers;
        public Color[] targetColors;
        public float duration = 1.0f;

        public void  ChangeColor()
        {
            targetColors[0].a = 1f;
            targetColors[1].a = 1f;
            
            spriteRenderers[0].DOColor(targetColors[0], duration)
                .SetLoops(-1, LoopType.Yoyo);
            spriteRenderers[1].DOColor(targetColors[1], duration)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

}