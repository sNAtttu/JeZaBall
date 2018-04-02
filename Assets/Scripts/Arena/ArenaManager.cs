using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class ArenaManager : MonoBehaviour
    {
        public List<GameObject> ArenaGridPiecesCache;

        private void Start()
        {
            ArenaGridPiecesCache = new List<GameObject>();
        }
    }
}

