using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFacePlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Follow only X and Z of player, Y remains unchanged
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                       transform.position.y,
                                       player.transform.position.z);
        // Calcualte rotational value
        this.transform.LookAt(targetPostition);

        // Make the box face the player
        transform.Rotate(0f, -90f, 0);
    }
}
