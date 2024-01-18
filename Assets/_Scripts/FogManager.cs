using System.Collections.Generic;
using UnityEngine;

namespace ReplayValue
{
    public class FogManager : Singleton<FogManager>
    {
        // public static FogManager Instance
        // {
        //     get
        //     {
        //         if (_instance == null && !_isApplicationQuitting)
        //         {
        //             _instance = FindObjectOfType<FogManager>();
        //             if (_instance == null)
        //             {
        //                 GameObject fogManagerObj = new GameObject("FogManager");
        //                 _instance = fogManagerObj.AddComponent<FogManager>();
        //             }
        //         }
        //         return _instance;
        //     }
        //     private set
        //     {
        //         _instance = value;
        //     }
        // }
        // private static FogManager _instance;
        // private static bool _isApplicationQuitting = false;
        // public static bool IsApplicationQuitting
        // {
        //     get { return _isApplicationQuitting; }
        // }

        [SerializeField] private FogTile fogTilePrefab;
        [SerializeField] private float fogGridSize = 50f;

        [SerializeField] private Transform groundPlaneTransform;

        private List<FogTile> fogTiles = new List<FogTile>();
        private List<IFogRevealer> fogRevealers = new List<IFogRevealer>();

        private void Update()
        {
            UpdateFog();
        }

        protected override void Awake()
        {
            base.Awake();
            GenerateFog();
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
                    var fogTile = Instantiate(fogTilePrefab, position, Quaternion.identity, transform);
                    fogTile.transform.localScale = new Vector3(cellSizeX, cellSizeY, 1);

                    fogTiles.Add(fogTile);
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
