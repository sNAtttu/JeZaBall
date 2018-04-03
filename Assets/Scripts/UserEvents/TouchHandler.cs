using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arena;

namespace InputEvents
{
    public class TouchHandler : MonoBehaviour
    {
        private ArenaManager arenaManager;
        //Change me to change the touch phase used.
        private TouchPhase touchPhase = TouchPhase.Began;

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hitInfo;
                // Create a particle if hit
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.tag == Utilities.Constants.TagGridPiece)
                    {
                        int x = hitInfo.collider.GetComponentInChildren<ArenaGridPiece>().CoordinateX;
                        int y = hitInfo.collider.GetComponentInChildren<ArenaGridPiece>().CoordinateY;

                        if(arenaManager == null)
                        {
                            arenaManager = FindObjectOfType<ArenaManager>();
                        }

                        arenaManager.HandleUserClick(x, y);

                    }
                }

            }
        }
    }
}


