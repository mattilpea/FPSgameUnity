using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;


public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform gunMuzzle;

    public GameObject bulletHolePrefab;
    public GameObject bulletHitParticlePrefab;

    public AudioSource audioSource;

    [SerializeField] GameObject muzzleFlash;
    [SerializeField] private TextMeshProUGUI gunStatusText; // Add TextMeshPro reference

    [HideInInspector] public bool isShooting = false;

    


    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Transform cameraTransform;

    public WeaponAnim weaponAnim;
    private CinemachineImpulseSource recoilShakeImpulseSource;


    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    private Vector3 targetRecoil = Vector3.zero;
    [HideInInspector] public Vector3 currentRecoil = Vector3.zero;


    private bool isReloading = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentAmmo = gunData.magazineSize;

        playerController = transform.root.GetComponent<PlayerController>();
        cameraTransform = playerController.virtualCamera.transform;
        weaponAnim = GetComponentInChildren<WeaponAnim>();

        audioSource = GetComponent<AudioSource>();
        recoilShakeImpulseSource = GetComponent<CinemachineImpulseSource>();
        UpdateGunStatus("Ready"); // Initial status
    }

    public virtual void Update()
    {
        playerController.ResetRecoil(gunData);

        MuzzleFlash(isShooting);
    }

    public void TryReload()
    {
        if (!isReloading && currentAmmo < gunData.magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log(gunData.gunName + "is reloading");
        UpdateGunStatus("Reloading...");

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magazineSize;
        isReloading = false;

        Debug.Log(gunData.gunName + "is reloaded");
        UpdateGunStatus("Reloaded");
    }
    
    public void TryShoot()
    {
        if (isReloading)
        {
            Debug.Log(gunData.gunName + "is reloading");
            UpdateGunStatus("Reloading...");
            return;
        }
        if (currentAmmo <= 0f)
        {
            Debug.Log(gunData.gunName + "has no bulets left, please reload (R) ");
            UpdateGunStatus("Out of Ammo! Reload!");
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1/gunData.fireRate);
            HandleShoot();
        }
    }

    private void HandleShoot()
    {

        isShooting = true;

        currentAmmo--;

        //recoil();
        //muzzleFlash();
        weaponAnim.isRecoiling = true;

        Debug.Log(gunData.gunName + "Shot!, Bullets left: " + currentAmmo);
        UpdateGunStatus("Shooting... Bullets left: " + currentAmmo);
        Shoot();

        playerController.ApplyRecoil(gunData);

        recoilShakeImpulseSource.GenerateImpulse();


        PlayFireSound();

    }

    public abstract void Shoot();
  
    private void MuzzleFlash(bool activate)
    {
        muzzleFlash.SetActive(activate);
    }

    private void PlayFireSound()
    {
        if (gunData.fireSound == null)
        {
            Debug.LogError("Fire sound is missing in GunData!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on " + gameObject.name);
            return;
        }

        audioSource.PlayOneShot(gunData.fireSound);
        Debug.Log("Playing fire sound!");
    }

    private void UpdateGunStatus(string status)
    {
        if (gunStatusText != null)
        {
            gunStatusText.text = status;
        }
    }
}
