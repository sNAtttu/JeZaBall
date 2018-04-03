using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Vector3 BallMovementDirection;
    public float MaxSpeed = 10f;

    private void Start()
    {
        BallMovementDirection = new Vector3(1, 0, 1);
    }

    private void Update()
    {
        MoveBall(BallMovementDirection);
    }

    private void MoveBall(Vector3 direction)
    {
        gameObject.transform.Translate(direction * Time.deltaTime * MaxSpeed);
    }


}
