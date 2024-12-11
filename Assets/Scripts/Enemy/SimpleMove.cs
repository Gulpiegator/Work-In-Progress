using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 movementAxis = Vector3.right;
    private Vector3 movementDirection;
    private Rigidbody rb;

    void Start(){
        movementDirection = movementAxis.normalized;

        //grab rigidbody and stop rotation
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate(){
        //transform.Translate(movementDirection * speed * Time.deltaTime);
        rb.velocity = movementDirection * speed;
    }

    private void OnCollisionEnter(Collision collision){
        //reverse on collision
        Vector3 reflectedDirection = Vector3.Reflect(movementDirection, collision.contacts[0].normal);

        if (movementAxis.x != 0) // Movement locked to X-axis
        {
            movementDirection = new Vector3(-movementDirection.x, 0, 0);
        }
        else if (movementAxis.y != 0) // Movement locked to Y-axis
        {
            movementDirection = new Vector3(0, -movementDirection.y, 0);
        }
        else if (movementAxis.z != 0) // Movement locked to Z-axis
        {
            movementDirection = new Vector3(0, 0, -movementDirection.z);
        }
    }
}