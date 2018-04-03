using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Vector3 StartDirection;
    public float MaxSpeed = 10f;

    private Vector3 BallMovementDirection;

    private void Start()
    {
        BallMovementDirection = StartDirection;
    }

    private void Update()
    {
        MoveBall(BallMovementDirection);
    }

    private void MoveBall(Vector3 direction)
    {
        gameObject.transform.Translate(direction * Time.deltaTime * MaxSpeed);
    }

    public void ChangeMovementDirection(Utilities.WallType wallHitted)
    {
        switch (wallHitted)
        {
            case Utilities.WallType.Bottom:
                BallMovementDirection.z *= -1;
                break;
            case Utilities.WallType.Top:
                BallMovementDirection.z *= -1;
                break;
            case Utilities.WallType.Left:
                BallMovementDirection.x *= -1;
                break;
            case Utilities.WallType.Right:
                BallMovementDirection.x *= -1;
                break;
            default:
                Debug.LogWarning("Unknown wall type");
                break;
        }
    }

}
