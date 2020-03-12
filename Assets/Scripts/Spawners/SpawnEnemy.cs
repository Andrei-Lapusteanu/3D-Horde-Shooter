using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private const float SPAWN_START_DELAY_MIN = 0.5f;
    private const float SPAWN_START_DELAY_MAX = 5.0f;
    private const float SPAWN_WAIT_MIN = 10.0f;
    private const float SPAWN_WAIT_MAX = 25.0f;

    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnerCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnerCoroutine()
    {
        for (; ; )
        {
            var randTime = Random.Range(SPAWN_WAIT_MIN, SPAWN_WAIT_MAX);
            yield return new WaitForSeconds(randTime);
            InstantiateEnemy();
        }
    }

    private void InstantiateEnemy()
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
        Enemy enemyCtrl = enemy.GetComponent<Enemy>();

        var randomType = Random.Range(0, 2);

        if (randomType == 0)
            enemyCtrl.isSpellcaster = true;
        else
            enemyCtrl.isSpellcaster = false;
    }
}
