using System;
using UnityEngine;

public class ShipMoving : MonoBehaviour
{
    public float velocity;
    public float targetVelocity;
    public float acceleration;

    private void Update()
    {
        Accelerate();
    }
    void Accelerate()
    {
        if (velocity < targetVelocity)
        {
            velocity += acceleration * Time.deltaTime;
        }
        else if (velocity > targetVelocity)
        {
            velocity -= acceleration * Time.deltaTime;
        }
        velocity = (float)Math.Round(velocity, 2);
    }
}
