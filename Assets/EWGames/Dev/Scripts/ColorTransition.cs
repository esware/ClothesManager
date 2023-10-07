using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace EWGames.Dev.Scripts
{
    [System.Serializable]
    public struct RendererColorPair
    {
        public SpriteRenderer renderer;
        public Color defaultColor;
        public Color targetColor;
        
    }
    public class ColorTransition : MonoBehaviour
    {
        public RendererColorPair[] rendererColorPairs;
        public SpriteRenderer icon;

        private Coroutine _coroutine;

        private void Start()
        {
            for (int i = 0; i < rendererColorPairs.Length; i++)
            {
                rendererColorPairs[i].defaultColor = rendererColorPairs[i].renderer.color;
                rendererColorPairs[i].targetColor.a = 1;
            }
        }

        public void ChangeColor()
        {
            if (_coroutine==null)
            {
                _coroutine=StartCoroutine(LerpColors());
            }
        }

        public void SetIcons(Sprite clothesIcon)
        {
            icon.sprite = clothesIcon;
        }

        public void ResetColor()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            foreach (var pair in rendererColorPairs)
            {
                pair.renderer.color = pair.defaultColor;
            }
        }

        private IEnumerator LerpColors()
        {
            while (true)
            {
                foreach (var pair in rendererColorPairs)
                {
                    Color lerpedColor = Color.Lerp(pair.defaultColor, pair.targetColor, Mathf.PingPong(Time.time, 1));
                    pair.renderer.color = lerpedColor;
                }
                yield return null;
            }
        }
    }

}