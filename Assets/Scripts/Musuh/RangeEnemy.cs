using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public float shootingRange;
    public GameObject fireBall;
    public float fireRate = 1f;
    private float nextFireTime;
    public GameObject fireParent;
    private Transform player;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }

        else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(fireBall, fireParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }


        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);
        if (timer < 4)
        {
            timer += Time.deltaTime;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
