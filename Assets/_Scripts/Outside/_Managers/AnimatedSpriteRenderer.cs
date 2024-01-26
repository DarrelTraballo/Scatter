using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    [System.Serializable]
    public class StateSprites
    {
        public string stateName;
        public Sprite[] animationSprites;
    }

    public class AnimatedSpriteRenderer : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        public StateSprites[] animationSprites;

        public string currentState;

        public float animationTime = 0.25f;
        private int animationFrame = 0;

        public bool shouldLoop = true;

        private void Awake()
        {
            spriteRenderer = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            if (animationSprites != null && animationSprites.Length > 0)
            {
                currentState = animationSprites[0].stateName;
            }
        }

        private void Start()
        {
            StartAnimation();
        }

        public void StartAnimation()
        {
            InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
        }

        private void NextFrame()
        {
            animationFrame++;
            var currentSprites = GetCurrentStateSprites();
            if (shouldLoop && animationFrame >= currentSprites.Length)
            {
                animationFrame = 0;
            }

            if (animationFrame >= 0 && animationFrame < currentSprites.Length)
                spriteRenderer.sprite = currentSprites[animationFrame];
        }

        public void ChangeState(string newState)
        {
            foreach (var stateSprite in animationSprites)
            {
                if (stateSprite.stateName == newState)
                {
                    currentState = newState;
                    animationFrame = 0;
                    return;
                }
            }
            StartAnimation();
        }

        private Sprite[] GetCurrentStateSprites()
        {
            foreach (var stateSprite in animationSprites)
            {
                if (stateSprite.stateName == currentState)
                {
                    return stateSprite.animationSprites;
                }
            }
            return null;
        }

        private void OnEnable()
        {
            spriteRenderer.enabled = true;
        }

        private void OnDisable()
        {
            spriteRenderer.enabled = false;
        }
    }
}
