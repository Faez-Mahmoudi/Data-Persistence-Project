using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float Speed = 6.0f;
    [SerializeField] private float MaxMovement = 3.25f;

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        pos.x += input * Speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            Speed = 10.0f;
        else 
            Speed = 6.0f;

        if (pos.x > MaxMovement)
            pos.x = MaxMovement;
        else if (pos.x < -MaxMovement)
            pos.x = -MaxMovement;

        transform.position = pos;
    }
}
