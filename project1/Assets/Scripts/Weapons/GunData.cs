using System.Numerics;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using System.Collections.Generic;

[CreateAssetMenu (fileName = "NewGunData", menuName = "Gun/GunData") ]

public class GunData : ScriptableObject
{
    public string gunName;

    public LayerMask targetLayerMask;

    [Header("Fire Config")]
    public float shootingRange;
    public float fireRate;

    [Header("Reload Config")]
    public float magazineSize;
    public float reloadTime;

    [Header("Recoil Settings")]
    public float recoilAmount;
    public UnityEngine.Vector2 maxRecoil;
    public float recoilSpeed;
    public float resetRecoilSpeed;

    [Header("VFX")]
    public GameObject bulletTrailPrefab;
    public float bulletSpeed;

    [Header("SFX")]
    public AudioClip fireSound;


}
