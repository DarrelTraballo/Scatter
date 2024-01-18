using UnityEngine;

namespace ReplayValue
{
    public class FogTile : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private bool isVisible = false;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.transform.Find("Tile").gameObject.SetActive(!isVisible);
        }

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (other.CompareTag("SquadViewRadius"))
        //     {
        //         gameObject.transform.Find("Tile").gameObject.SetActive(false);
        //     }
        // }

        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (other.CompareTag("SquadViewRadius"))
        //     {
        //         gameObject.transform.Find("Tile").gameObject.SetActive(true);
        //     }
        // }

        // private void OnTriggerStay2D(Collider2D other)
        // {
        //     if (other.CompareTag("SquadViewRadius"))
        //     {
        //         gameObject.transform.Find("Tile").gameObject.SetActive(false);
        //     }
        // }
    }
}
