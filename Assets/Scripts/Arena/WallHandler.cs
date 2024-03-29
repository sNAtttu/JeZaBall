﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class WallHandler : MonoBehaviour
    {

        public int CoordinateX;
        public int CoordinateY;

        public Utilities.WallType WallType;
        public Utilities.WallState WallState;

        private static Animator wallAnimator;

        private Guid wallId;
        private WallCreationScript wallCreation;

        private void Awake()
        {
            wallId = Guid.NewGuid();
        }

        private void Start()
        {
            wallAnimator = GetComponent<Animator>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == Utilities.Constants.TagBall)
            {
                if(WallState == Utilities.WallState.Creating)
                {
                    if(wallCreation == null)
                    {
                        wallCreation = FindObjectOfType<WallCreationScript>();
                    }
                    wallCreation.SetWallHittedOnCreation(gameObject);
                }
            }
        }

        public static void PlayTriggerAnimation(string trigger)
        {
            wallAnimator.SetTrigger(trigger);
        }

    }
}