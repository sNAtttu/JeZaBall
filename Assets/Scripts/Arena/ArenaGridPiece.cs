using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class ArenaGridPiece : MonoBehaviour
    {
        public int CoordinateX;

        public int CoordinateY;

        private ArenaManager arenaManager;
        // For debugging
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (arenaManager == null)
                {
                    arenaManager = FindObjectOfType<ArenaManager>();
                }
                arenaManager.HandleUserClick(CoordinateX, CoordinateY);
            }
        }

    }
}

