using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPickup : MonoBehaviour
{
    private AudioSource audioBoxPickup;
    private PlayerController player;

    private int ammoAmount = 60;
    private int healthAmount = 40;

    private void Start()
    {
        audioBoxPickup = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        // If collider is triggered by the player
        if (other.gameObject.tag == "Player")
        {
            // Add ammo to player
            if (gameObject.name == "ammo_box(Clone)")
            {
                player.IncreaseAmmo(ammoAmount);
                ManageObjectDestruction();
            }

            // Add health to player
            else if (gameObject.name == "med_box(Clone)")
                if (player.IsFullHealth() == false)
                {
                    player.Heal(healthAmount);
                    ManageObjectDestruction();
                }
        }
    }

    private void ManageObjectDestruction()
    {

        // Play pickup sound
        AudioSource.PlayClipAtPoint(audioBoxPickup.clip, transform.position);

        // Destroy object
        Destroy(this.gameObject);

        // Reset respawn counter
        ItemSpawnerController ISController = GameObject.Find("ISController").GetComponent<ItemSpawnerController>();
        ISController.ResetSpawner(transform.parent.name);
    }
}
