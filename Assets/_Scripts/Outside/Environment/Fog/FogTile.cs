using UnityEngine;

namespace ReplayValue
{
    public class FogTile : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.transform.Find("Tile").gameObject.SetActive(isVisible);
        }
    }
}
