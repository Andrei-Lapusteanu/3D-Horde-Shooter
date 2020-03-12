using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    private const float SPAWN_WAIT_MIN = 15.0f;
    private const float SPAWN_WAIT_MAX = 35.0f;
    AudioSource audioBoxSpawn;

    // Start is called before the first frame update
    void Start()
    {
        audioBoxSpawn = GetComponent<AudioSource>();
    }

    public IEnumerator SpawnBoxForPlayersNeeds(GameObject ammoBox, GameObject medBox)
    {
        // Wait
        var randTime = Random.Range(SPAWN_WAIT_MIN, SPAWN_WAIT_MAX);
        yield return new WaitForSeconds(randTime);

        // RNG
        int randNo = Random.Range(0, 100);
        int playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetHealthPoints();

        // Spawn item which player would need more
        if (playerHP >= 90)
        {
            // 10% chance for health drop
            if (randNo <= 10)
                SpawnItemBox(medBox);
            else SpawnItemBox(ammoBox);
        }
        else if (playerHP >= 75 && playerHP < 90)
        {
            // 20% chance for health drop
            if (randNo <= 20)
                SpawnItemBox(medBox);
            else SpawnItemBox(ammoBox);
        }
        else if (playerHP >= 60 && playerHP < 75)
        {
            // 30% chance for health drop
            if (randNo <= 30)
                SpawnItemBox(medBox);
            else SpawnItemBox(ammoBox);
        }
        else if (playerHP >= 40 && playerHP < 60)
        {
            // 40% chance for health drop
            if (randNo <= 40)
                SpawnItemBox(medBox);
            else SpawnItemBox(ammoBox);
        }
        else if (playerHP >= 20 && playerHP < 40)
        {
            // 60% chance for health drop
            if (randNo <= 60)
                SpawnItemBox(medBox);
            else SpawnItemBox(ammoBox);
        }
        else if (playerHP < 20)
        {
            // 75% chance for health drop
            if (randNo <= 75)
                SpawnItemBox(medBox);
            else SpawnItemBox(ammoBox);
        }
    }

    public void SpawnItemBox(GameObject box)
    {
        GameObject boxClone = Instantiate(box, transform.position, Quaternion.identity);
        boxClone.transform.parent = gameObject.transform;

        AudioSource.PlayClipAtPoint(audioBoxSpawn.clip, transform.position);
    }
}
