using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField]private int damage = 10;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<HealthMusuh>() != null)
        {
            HealthMusuh health = collider.GetComponent<HealthMusuh>();
            health.Damage(damage);
        }

        if (collider.GetComponent<HealthTembok>() != null)
        {
            HealthTembok health = collider.GetComponent<HealthTembok>();
            health.Damage(damage);
        }
    }
}

