using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    private int currentHealth;

    [SerializeField] AudioClip deathSound;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die(){
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        Destroy(gameObject);
    }
}
