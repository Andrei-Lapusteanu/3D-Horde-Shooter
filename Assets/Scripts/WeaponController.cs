using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // FSM
    public static int WEAPON_STATE_IDLE = 0;
    public static int WEAPON_STATE_FIRE = 1;
    public static int WEAPON_STATE_REALOAD = 2;
    private static int currentState = -1;

    // Components
    static AudioSource[] audioSources;
    static AudioSource audioFire;
    static AudioSource audioReload;
    static Animator animator;
    static ParticleSystem particleSys;
    RaycastController raycaster;

    // Weapon vars
    private const int INIT_BULLET_COUNT = 240;
    private const int CLIP_SIZE = 30;
    private int totalBulletCount;
    private int clipBulletCount;
    private int leftoverBulletCount;
    private readonly float fireRate = 0.09f;
    private float nextFire = 0.0f;
    private bool justReloaded = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        animator = GetComponent<Animator>();
        particleSys = GetComponentInChildren<ParticleSystem>();
        audioSources = GetComponents<AudioSource>();
        audioFire = audioSources[0];
        audioReload = audioSources[1];
        raycaster = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RaycastController>();

        totalBulletCount = INIT_BULLET_COUNT;
        clipBulletCount = CLIP_SIZE;
        leftoverBulletCount = totalBulletCount - clipBulletCount;
    }

    private void Update()
    {
        //DisplayBullets();

        if (Input.GetButton("Fire1") && IsReloadingAnim() == false)
        {
            if (IsClipEmpty() == false)
            {
                if (Time.time > nextFire)
                {
                    raycaster.RaycastFireWeapon(nextFire);

                    UpdateState(WEAPON_STATE_FIRE);
                    nextFire = Time.time + (fireRate * 1.6666f);
                    raycaster.IncrementBulletBurstCount();
                    justReloaded = false;
                }
            }

            if (IsClipEmpty() == true && justReloaded == false)
            {
                UpdateState(WEAPON_STATE_REALOAD);
                raycaster.ResetBulletBurstCount();
                justReloaded = true;
            }
        }
        else
        {
            UpdateState(WEAPON_STATE_IDLE);
            raycaster.ResetBulletBurstCount();
        }

        raycaster.RaycastEnemyHealth();
    }

    public void UpdateState(int stateVal)
    {
        animator.SetInteger("state", currentState = stateVal);

        if (stateVal == WEAPON_STATE_FIRE)
            FireWeapon();

        else if (stateVal == WEAPON_STATE_REALOAD)
            ReloadWeapon();
    }

    private void FireWeapon()
    {
        if (IsClipEmpty() == false)
        {
            clipBulletCount--;
            totalBulletCount--;
        }

        // Audio stuff
        audioFire.pitch = Random.Range(0.85f, 1.05f);
        audioFire.Play();
        particleSys.Play();
    }

    public void ReloadWeapon()
    {
        // Audio stuff
        if (!audioReload.isPlaying)
            audioReload.Play();

        // Clip reload and wait until animation finishes
        StartCoroutine(WaitForReloadRoutine());
    }

    IEnumerator WaitForReloadRoutine()
    {
        yield return new WaitUntil(() => audioReload.isPlaying == false);

        if(totalBulletCount <= CLIP_SIZE)
        {
            clipBulletCount = totalBulletCount;
            leftoverBulletCount = 0;
        }
        else
        {
            clipBulletCount = CLIP_SIZE;
            leftoverBulletCount = totalBulletCount - clipBulletCount;
        }
    }

    public void IncreaseAmmo(int ammoAmount)
    {
        totalBulletCount += ammoAmount;
        leftoverBulletCount = totalBulletCount - clipBulletCount;

        if(clipBulletCount == 0)
            StartCoroutine(WaitForReloadRoutine());
    }

    public static bool IsReloadingAnim()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Reload_2"))
            return true;
        else return false;
    }

    public bool IsClipEmpty()
    {
        if (clipBulletCount == 0)
            return true;
        else return false;
    }

    public bool IsClipFull()
    {
        if (clipBulletCount == CLIP_SIZE)
            return true;
        else
            return false;
    }

    public int GetCurrentState()
    {
        return currentState;
    }

    public void DisplayBullets()
    {
        //Debug.Log("Bullets: " + clipBulletCount + "/" + leftoverBulletCount);
    }

    public int GetBulletCount()
    {
        return totalBulletCount;
    }

    public int GetClipCount()
    {
        return clipBulletCount;
    }

    public int GetLeftoverBulletsCount()
    {
        return leftoverBulletCount;
    }
}
