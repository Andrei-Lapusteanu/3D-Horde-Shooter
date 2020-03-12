using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerController : MonoBehaviour
{
    public GameObject ammoBox;
    public GameObject medBox;

    private const string spawner_1 = "ItemSpawner_1";
    private const string spawner_2 = "ItemSpawner_2";
    private const string spawner_3 = "ItemSpawner_3";

    private List<SpawnBox> spawners;

    // Start is called before the first frame update
    void Start()
    {
        spawners = new List<SpawnBox>() { 
            GameObject.Find("ItemSpawner_1").GetComponent<SpawnBox>(),
            GameObject.Find("ItemSpawner_2").GetComponent<SpawnBox>(),
            GameObject.Find("ItemSpawner_3").GetComponent<SpawnBox>()
        };


        foreach (SpawnBox spawner in spawners)
            spawner.StartCoroutine(spawner.SpawnBoxForPlayersNeeds(ammoBox, medBox));
    }

    public void ResetSpawner(string spawnerName)
    {
        switch(spawnerName)
        {
            case spawner_1:
                spawners[0].StartCoroutine(spawners[0].SpawnBoxForPlayersNeeds(ammoBox, medBox));
                break;

            case spawner_2:
                spawners[1].StartCoroutine(spawners[1].SpawnBoxForPlayersNeeds(ammoBox, medBox));
                break;

            case spawner_3:
                spawners[2].StartCoroutine(spawners[2].SpawnBoxForPlayersNeeds(ammoBox, medBox));
                break;

            default:
                break;
        }
    }
}
