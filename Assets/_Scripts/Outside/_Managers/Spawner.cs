using UnityEngine;

namespace ReplayValue
{

    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Transform groundPlaneTransform;
        [SerializeField] private int amountToSpawn;

        public void Spawn(string tag)
        {
            for (var i = 0; i < amountToSpawn; i++)
            {
                float maxBoundsX = groundPlaneTransform.localScale.x / 2;
                float maxBoundsY = groundPlaneTransform.localScale.y / 2;

                float randomX = Random.Range(-maxBoundsX + 75, maxBoundsX - 75);
                float randomY = Random.Range(-maxBoundsY + 75, maxBoundsY - 75);

                Vector2 spawnPos = new Vector2(randomX, randomY);

                var spawnedObject = ObjectPooler.Instance.GetPooledObject(tag);

                if (spawnedObject != null)
                {
                    spawnedObject.transform.position = spawnPos;
                    spawnedObject.SetActive(true);
                }
            }
        }

    }
}
