using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class FogManager : MonoBehaviour
    {
        public static FogManager Instance { get; private set; }
        #region Singleton
        private FogManager() { }
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            GenerateFog();

        }
        #endregion

        [Header("Debug")]
        [SerializeField] private bool isDebugMode = false;

        [SerializeField] private FogTile fogTilePrefab;
        [SerializeField] private float fogGridSize = 50f;

        [SerializeField] private Transform groundPlaneTransform;

        private List<FogTile> fogTiles = new List<FogTile>();
        private List<IFogRevealer> fogRevealers = new List<IFogRevealer>();

        private void Update()
        {
            UpdateFog();
        }

        private void GenerateFog()
        {
            float cellSizeX = groundPlaneTransform.localScale.x / fogGridSize;
            float cellSizeY = groundPlaneTransform.localScale.y / fogGridSize;


            for (var x = 0; x < fogGridSize; x++)
            {
                for (var y = 0; y < fogGridSize; y++)
                {
                    float posX = x * cellSizeX + (cellSizeX / 2) - (groundPlaneTransform.localScale.x / 2);
                    float posY = y * cellSizeY + (cellSizeY / 2) - (groundPlaneTransform.localScale.y / 2);

                    Vector3 position = new Vector3(posX, posY, 0);
                    FogTile fogTile = Instantiate(fogTilePrefab, position, Quaternion.identity, transform);
                    fogTile.transform.localScale = new Vector3(cellSizeX, cellSizeY, 1);

                    fogTiles.Add(fogTile);

                    if (isDebugMode)
                    {
                        var fogTileColor = fogTile.GetComponentInChildren<SpriteRenderer>().color;
                        fogTileColor.a = 0.5f;
                        fogTile.GetComponentInChildren<SpriteRenderer>().color = fogTileColor;
                    }
                }
            }
        }

        private void UpdateFog()
        {
            foreach (var fogTile in fogTiles)
            {
                fogTile.SetVisible(false);
            }

            foreach (var revealer in fogRevealers)
            {
                foreach (var fogTile in fogTiles)
                {
                    float distance = Vector3.Distance(revealer.Position, fogTile.transform.position);
                    if (distance <= revealer.ViewDistance)
                    {
                        fogTile.SetVisible(true);
                    }
                }
            }
        }

        public void RegisterFogRevealer(IFogRevealer revealer)
        {
            if (!fogRevealers.Contains(revealer))
            {
                fogRevealers.Add(revealer);
            }
        }

        public void UnregisterFogRevealer(IFogRevealer revealer)
        {
            fogRevealers.Remove(revealer);
        }
    }
}
