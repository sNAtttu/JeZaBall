using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arena;

namespace Ball
{
    public class BallSetup : MonoBehaviour
    {
        public GameObject Ball;

        private ArenaManager arenaManager;

        public void SpawnBall()
        {
            if(arenaManager == null)
            {
                arenaManager = FindObjectOfType<ArenaManager>();
            }
            Vector3 spawnPiecePosition = arenaManager.GetRandomArenaGridPiece().transform.position;
            Vector3 ballSpawnPosition = new Vector3(spawnPiecePosition.x,
                spawnPiecePosition.y + Ball.transform.localScale.y,
                spawnPiecePosition.z);

            Instantiate(Ball, ballSpawnPosition, Ball.transform.rotation);
            arenaManager.InitComplete();
        }

    }
}