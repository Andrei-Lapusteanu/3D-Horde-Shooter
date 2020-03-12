using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public GameObject healthBarImg;
    public GameObject projectile;
    public bool isSpellcaster;

    const int STATE_WALK = 0;
    const int STATE_ATTACK_MELEE = 1;
    const int STATE_ATTACK_RANGED = 2;
    const int STATE_DEATH = 3;

    const float AGENT_WALK_SPEED = 5.25f;
    const float AGENT_ATTACK_WALK_SPEED = 2.75f;

    const int MIN_MELEE_DEALT_DAMAGE = 7;
    const int MAX_MELEE_DEALT_DAMAGE = 12;
    const int MIN_TAKEN_DAMAGE = 8;
    const int MAX_TAKEN_DAMAGE = 12;

    const int MIN_POSSIBLE_HEALTH = 90;
    const int MAX_POSSIBLE_HEALH = 140;

    const float PUSH_BACK_FORCE = 10f;
    const float SPELLCASTER_MIN_DIST = 8f;
    const float SPELLCASTER_MAX_DIST = 20f;

    private float startingHP;
    private float HP;

    private PlayerController player;
    private Animator animator;
    private Canvas canvasStats;
    private NavMeshAgent navAgent;
    private Transform playerTransform;
    private Rigidbody rigidBody;
    private RaycastController raycaster;
    private GameObject playerCamera;

    private float nextPlayerDamage = 0.0f;
    private float playerDamageRate = 1.0f;

    private float nextPlayerRangedDamage = 0.0f;
    private float playerRagDamageRate = 2.21f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        canvasStats = GetComponentInChildren<Canvas>();
        navAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        raycaster = GetComponentInChildren<RaycastController>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        startingHP = Random.Range(MIN_POSSIBLE_HEALTH, MAX_POSSIBLE_HEALH);
        HP = startingHP;
    }

    // Update is called once per frame
    void Update()
    {
        healthBarImg.transform.localScale = new Vector3(HP / startingHP, 1, 1);

        // Attack up close
        if (isSpellcaster == false)
            MeleeNecromancerRoutine();

        // Spellcaster from a distance
        else if (isSpellcaster == true)
            RangedNecromancerRoutine();

    }

    // Physics system update, regular Update() will not work for this 
    private void FixedUpdate()
    {
        HideStats();
    }

    private void MeleeNecromancerRoutine()
    {
        if (HP > 0)
        {
            if (GetDistanceToPlayer() > 5f)
                WalkToPlayer();
            else
                AttackMelee();
        }
        else
            Die();
    }

    private void RangedNecromancerRoutine()
    {
        if (HP > 0)
        {
            // If player is in line of sight
            if (GetDistanceToPlayer() >= SPELLCASTER_MIN_DIST &&
                GetDistanceToPlayer() <= SPELLCASTER_MAX_DIST)
            {
                if (raycaster.RaycastEnemyLineOfSight())
                {
                    LookAtPlayer();
                    navAgent.isStopped = true;

                    // Spell attack
                    AttackRanged();
                }

                // Relocate enemy
                else
                {
                    WalkToPlayer();
                    navAgent.speed = AGENT_ATTACK_WALK_SPEED + 5f;
                    navAgent.isStopped = false;

                    if (GetDistanceToPlayer() < SPELLCASTER_MIN_DIST + 1)
                    {
                        // Just wait
                        navAgent.isStopped = true;
                        LookAtPlayer();
                    }
                }
            }

            // Get closer to player
            else if (GetDistanceToPlayer() > SPELLCASTER_MAX_DIST)
            {
                WalkToPlayer();
                navAgent.isStopped = false;
            }

            // Move away, no melee attack for spellcaster
            else if (GetDistanceToPlayer() < SPELLCASTER_MIN_DIST)
            {
                animator.SetInteger("EnemyState", STATE_WALK);
                navAgent.SetDestination(GameObject.FindGameObjectWithTag("OutsideWayPoint").transform.position);
                navAgent.isStopped = false;
            }
        }
        else
            Die();
    }

    public float GetDistanceToPlayer()
    {
        return (playerTransform.position - transform.position).magnitude;
    }

    public void TakeDamage(Vector3 hitDirection)
    {
        // Deplete health points
        HP -= Random.Range(MIN_TAKEN_DAMAGE, MAX_TAKEN_DAMAGE + 1);
        ChangeHealthBarColor();

        // Push back
        rigidBody.AddForce(-hitDirection * PUSH_BACK_FORCE, ForceMode.Impulse);
    }

    private void WalkToPlayer()
    {
        animator.SetInteger("EnemyState", STATE_WALK);
        navAgent.SetDestination(playerTransform.position);
        navAgent.speed = AGENT_WALK_SPEED;
    }

    private void AttackMelee()
    {
        if (GetDistanceToPlayer() < 3f)
            navAgent.isStopped = true;
        else
            navAgent.isStopped = false;

        animator.SetInteger("EnemyState", STATE_ATTACK_MELEE);
        navAgent.SetDestination(playerTransform.position);
        navAgent.speed = AGENT_ATTACK_WALK_SPEED;

        StartCoroutine(PlayerMeleeDamageDelay());
    }

    private void AttackRanged()
    {
        navAgent.isStopped = true;
        animator.SetInteger("EnemyState", STATE_ATTACK_RANGED);

        StartCoroutine(PlayerRangedDamageDelay());
    }

    IEnumerator PlayerMeleeDamageDelay()
    {
        yield return new WaitForSeconds(0.35f);

        if (Time.time > nextPlayerDamage)
        {
            nextPlayerDamage = Time.time + playerDamageRate;

            if (GetDistanceToPlayer() <= 4f && animator.GetInteger("EnemyState") == STATE_ATTACK_MELEE)
                player.TakeDamage(Random.Range(MIN_MELEE_DEALT_DAMAGE, MAX_MELEE_DEALT_DAMAGE + 1));
        }
    }

    IEnumerator PlayerRangedDamageDelay()
    {
        yield return new WaitForSeconds(0.8f);

        if (Time.time > nextPlayerRangedDamage)
        {
            nextPlayerRangedDamage = Time.time + playerRagDamageRate;

            if (GetCurrentState() == STATE_ATTACK_RANGED)
                Instantiate(projectile, raycaster.transform.position + (raycaster.transform.forward * 2f) + new Vector3(0f, -.25f, 0f), raycaster.transform.rotation);
            else
                nextPlayerRangedDamage = 0.0f;
        }
    }

    private void Die()
    {
        navAgent.isStopped = true;
        animator.SetInteger("EnemyState", STATE_DEATH);

        // Destroy health bar graphic
        Destroy(canvasStats.gameObject);

        // Disable collider 
        GetComponent<BoxCollider>().enabled = false;

        // Increase kill count
        player.IncrementKillCount();

        // Destroy body after 15 seconds
        Destroy(this.gameObject, 15f);
    }

    private void LookAtPlayer()
    {
        transform.LookAt(playerCamera.transform);
        raycaster.transform.LookAt(playerCamera.transform);
    }

    private void ChangeHealthBarColor()
    {
        if (HP > startingHP / 2)
        {
            float redness = ExtensionMethods.Remap(HP, startingHP, startingHP / 2, 0, 1);
            healthBarImg.GetComponent<Image>().color = new Color(redness, 1, 0);
        }
        else if (HP == startingHP / 2)
            healthBarImg.GetComponent<Image>().color = new Color(255, 255, 0);

        else
        {
            float greeness = ExtensionMethods.Remap(HP, 0, startingHP / 2, 0, 1);
            healthBarImg.GetComponent<Image>().color = new Color(1, greeness, 0);
        }
    }

    public void ShowStats()
    {
        canvasStats = GetComponentInChildren<Canvas>();
        canvasStats.enabled = true;
    }

    public void HideStats()
    {
        canvasStats = GetComponentInChildren<Canvas>();
        canvasStats.enabled = false;
    }

    public int GetCurrentState()
    {
        return animator.GetInteger("EnemyState");
    }
}


public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}