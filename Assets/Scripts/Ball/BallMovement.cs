using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Vector3 StartDirection;
    public float MaxSpeed = 10f;

    private static PlayMakerFSM ballFsm;

    private Vector3 BallMovementDirection;
    private Rigidbody ballBody;

    private void Start()
    {
        ballFsm = GetComponent<PlayMakerFSM>();
        ballBody = GetComponent<Rigidbody>();
        BallMovementDirection = StartDirection;
    }

    private void FixedUpdate()
    {
        ballBody.velocity = (BallMovementDirection * MaxSpeed);
    }

    public void SendBallFsmEvent(string fsmEvent)
    {
        ballFsm.SendEvent(fsmEvent);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == Utilities.Constants.TagWall)
        {
            SendBallFsmEvent(Utilities.Constants.Ball.EventHitWall);
            BallMovementDirection = Vector3.Reflect(BallMovementDirection, collision.contacts[0].normal);
            SendBallFsmEvent(Utilities.Constants.Ball.EventChangeDirection);
        }
    }

}
