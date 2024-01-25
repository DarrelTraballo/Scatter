using UnityEngine;

namespace ReplayValue
{

    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Transform groundPlaneTransform;
        [SerializeField] private int amountToSpawn;

        private void Start()
        {
            Spawn("Zombie");
        }

        private void Spawn(string tag)
        {
            for (var i = 0; i < amountToSpawn; i++)
            {
                float boundsX = groundPlaneTransform.localScale.x / 2;
                float boundsY = groundPlaneTransform.localScale.y / 2;

                Vector2 spawnPos = new Vector2(Random.Range(-boundsX, boundsX), Random.Range(-boundsY, boundsY));

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
