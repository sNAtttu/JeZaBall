using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class ArenaSetupScript : MonoBehaviour
    {
        [Tooltip("Width of the arena")]
        public int ArenaWidth = 10;

        [Tooltip("Height of the arena")]
        public int ArenaHeight = 10;

        [Tooltip("Pieces which are used in creation")]
        public GameObject ArenaGridPiece;

        private ArenaManager arenaManager;

        // Use this for initialization
        void Start()
        {
            arenaManager = GetComponent<ArenaManager>();
            CreateArenaGrid(ArenaWidth, ArenaHeight);
        }

        private void CreateArenaGrid(int width, int height)
        {
            Vector3 spawnPosition = new Vector3();
            List<GameObject> createdObjects = new List<GameObject>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject createdPiece = Instantiate(ArenaGridPiece,
                        spawnPosition,
                        ArenaGridPiece.transform.rotation,
                        gameObject.transform);

                    createdPiece.GetComponent<ArenaGridPiece>().CoordinateX = j;
                    createdPiece.GetComponent<ArenaGridPiece>().CoordinateY = i;
                    createdObjects.Add(createdPiece);
                    spawnPosition.x++;
                }
                spawnPosition.z++;
                spawnPosition.x = 0;
            }
            arenaManager.ArenaGridPiecesCache = createdObjects;
        }

    }
}