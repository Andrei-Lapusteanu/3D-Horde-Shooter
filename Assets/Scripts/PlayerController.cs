using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const int MAX_HP = 100;
    public int HP;

    private const float DEFAULT_MOVE_SPEED = 4.0f;
    private const float SPRINT_VEL_BOOST = 1.5f;

    private AudioSource[] audioSources;
    private AudioSource audioFlashlightClick;
    private AudioSource audioPlayerHit;

    private Light flashLight;
    private WeaponController weaponCtrl;
    private CharacterController characterCtrl;
    private Vector3 vectorMove;
    private int killCount = 0;

    public float moveSpeed = DEFAULT_MOVE_SPEED;
    public float jumpHeight = 0.18f;
    public float gravityForce = 3.25f;


    // Start is called before the first frame update
    void Start()
    {
        HP = MAX_HP;
        weaponCtrl = GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<WeaponController>();
        characterCtrl = GetComponent<CharacterController>();
        flashLight = GetComponentInChildren<Light>();

        audioSources = GetComponents<AudioSource>();
        audioFlashlightClick = audioSources[0];
        audioPlayerHit = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HP: " + HP);

        if (characterCtrl.isGrounded)
        {
            // Player ground movement
            vectorMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            vectorMove = transform.TransformDirection(vectorMove);

            // Jump
            if (Input.GetButton("Jump"))
                vectorMove.y += Mathf.Sqrt(jumpHeight * 2f * gravityForce);

            // Sprint
            if (Input.GetKey(KeyCode.LeftShift))
                moveSpeed = DEFAULT_MOVE_SPEED + SPRINT_VEL_BOOST;
            else moveSpeed = DEFAULT_MOVE_SPEED;
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && weaponCtrl.GetCurrentState() != WeaponController.WEAPON_STATE_FIRE)
            if (weaponCtrl.IsClipFull() == false)
                weaponCtrl.UpdateState(WeaponController.WEAPON_STATE_REALOAD);

        // Flashlight
        if (Input.GetKeyDown(KeyCode.F))
            ToggleFlashlight();

        // Apply gravity
        vectorMove.y -= gravityForce * Time.deltaTime;

        // Apply movement
        characterCtrl.Move(vectorMove * Time.deltaTime * moveSpeed);
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        audioPlayerHit.Play();
    }

    public void Heal(int healAmount)
    {
        if (HP + healAmount > 100)
            HP = 100;
        else
            HP += healAmount;
    }

    public void IncreaseAmmo(int ammoAmount)
    {
        weaponCtrl.IncreaseAmmo(ammoAmount);
    }

    public void ToggleFlashlight()
    {
        if (flashLight.enabled == true)
            flashLight.enabled = false;

        else
            flashLight.enabled = true;

        audioFlashlightClick.Play();
    }

    public int GetHealthPoints()
    {
        return HP;
    }

    public bool IsFullHealth()
    {
        if (HP == MAX_HP)
            return true;
        else
            return false;
    }

    public int GetBulletCount()
    {
        return weaponCtrl.GetBulletCount();
    }

    public int GetClipCount()
    {
        return weaponCtrl.GetClipCount();
    }

    public int GetLeftoverBulletsCount()
    {
        return weaponCtrl.GetLeftoverBulletsCount();
    }

    public void IncrementKillCount()
    {
        killCount++;
    }

    public int GetKillCount()
    {
        return killCount;
    }
}
