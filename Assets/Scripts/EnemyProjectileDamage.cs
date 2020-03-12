using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileDamage : MonoBehaviour
{
    const int MIN_RANGED_DEALT_DAMAGE = 10;
    const int MAX_RANGED_DEALT_DAMAGE = 15;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            player.TakeDamage(Random.Range(MIN_RANGED_DEALT_DAMAGE, MAX_RANGED_DEALT_DAMAGE));
    }
}
