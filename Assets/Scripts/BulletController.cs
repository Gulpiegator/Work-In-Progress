using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 direction; // Direction of the bullet
    [SerializeField] private float speed = 5f;
    [SerializeField] private int bulletDamage = 10; // Damage dealt by the bullet

    [SerializeField] private float lifetime = 3f;
    private Rigidbody rb; // Reference to the Rigidbody

    // Set the firing direction
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Apply velocity based on direction
        rb.velocity = direction * speed;

        // Ensure continuous collision detection
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Destroy(gameObject, lifetime);
    }

    // Detect collisions
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet collided with {collision.gameObject.name}");
        // Check for enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit an enemy!");

            // Reduce enemy health
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
        else
        {
            // Destroy the bullet on any other collision
            Destroy(gameObject);
        }
    }
}
