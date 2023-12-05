using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMusuh : MonoBehaviour
{
    [Header("Damage L, Heal K")]
    [SerializeField]public int health = 100;
    [SerializeField] private int MAXHealth = 100;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Damage(10);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Heal(10);
        }
    }

    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Damage Can't be negative");
        }

        this.health -= amount;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Heal Can't be negative");
        }

        bool wouldbeOverMaxHealth = health + amount > MAXHealth;

        if (wouldbeOverMaxHealth)
        {
            this.health = MAXHealth;
        }
        else
        {
            this.health += MAXHealth;
        }
        this.health -= amount;
    }

    private void Die()
    {
        Debug.Log("Object Died");
        Destroy(gameObject);
    }
}
