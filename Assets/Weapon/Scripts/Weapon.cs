using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private string _weaponName;
    [SerializeField] private int _maxAmmo = 30;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _reloadTime = 1;
    public float FireRate = 0.2f;

    [Header("Shooting Settings")]
    public Transform FirePoint;
    [SerializeField] private float _range = 50f;

    [Header("Effects")]
    [SerializeField] private ObjectPool _bulletTrailPool;
    [SerializeField] private ObjectPool _hitDecalPool;
    [SerializeField] private MuzzleFlash _muzzleFlash;

    private int _currentAmmo;
    [SerializeField] private bool _canShoot = true;
    private bool _isReloading = false;

    public event Action OnCurrAmmoChanged;

    private bool _isEquiped = false;
    private void Awake()
    {
        _currentAmmo = _maxAmmo;
    }
    public bool IsEquiped => _isEquiped;
    public string WeaponName => _weaponName;
    public int CurrentAmmo => _currentAmmo;
    public int MaxAmmo => _maxAmmo;
    public float Range => _range;

    public void Shoot(Vector3 position, Vector3 direction)
    {
        if (!_canShoot || _isReloading || _currentAmmo <= 0) return;

        _currentAmmo--;
        OnCurrAmmoChanged?.Invoke();
        PerformRaycast(position, direction);

        if (_muzzleFlash != null)
        {
            _muzzleFlash.PlayFlash();
            StartCoroutine(StopMuzzleFlash());
        }

        StartCoroutine(FireCooldown());
    }
    public void Reloaded()
    {
        if (_isReloading || _currentAmmo == _maxAmmo) return;

        _currentAmmo = _maxAmmo;
        _isReloading = false;
        _canShoot = true;
    }
    public IEnumerator Reload()
    {
        if (_isReloading || _currentAmmo == _maxAmmo) yield break;
        _isReloading = true;
        _canShoot = false;

        yield return new WaitForSeconds(_reloadTime);

        _currentAmmo = _maxAmmo;
        OnCurrAmmoChanged?.Invoke();
        _isReloading = false;
        _canShoot = true;
    }

    private void PerformRaycast(Vector3 position, Vector3 direction)
    {
        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        Vector3 endPoint = position + direction * _range;

        if (Physics.Raycast(ray, out hit, _range))
        {
            HitBox targetHitBox = hit.collider.GetComponent<HitBox>();
            if (targetHitBox != null)
            {
                targetHitBox.OnRaycastHit(_damage);
            }

            Rigidbody targetRb = hit.collider.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                targetRb.AddForce(direction * 2000);
            }

            if (hit.collider.gameObject.layer == 7)
            {
                GameObject decal = _hitDecalPool.GetObject();
                decal.GetComponent<HitDecal>()?.Initialize(hit.point, hit.normal, _hitDecalPool, hit.transform);
            }


            endPoint = hit.point;
        }

        GameObject trail = _bulletTrailPool.GetObject();
        trail.transform.position = FirePoint.position;
        trail.transform.rotation = Quaternion.LookRotation(direction);
        trail.GetComponent<BulletTrail>()?.Initialize(FirePoint.position, endPoint);
    }

    private IEnumerator FireCooldown()
    {
        _canShoot = false;
        yield return null;
        yield return new WaitForSeconds(FireRate);
        _canShoot = true;
    }

    private IEnumerator StopMuzzleFlash()
    {
        yield return new WaitForSeconds(FireRate);
        _muzzleFlash.StopFlash();
    }

    public void EquipWeapon(Transform _weaponHolder = null)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
        if (_weaponHolder != null)
        {
            this.transform.SetParent(_weaponHolder);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
        }
        Transform[] transform = GetComponentsInChildren<Transform>();

        foreach (Transform t in transform)
        {
            t.gameObject.layer = 3;
        }

        _isEquiped = true;
    }

    public void UnequipWeapon()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;

        this.transform.SetParent(null);

        Transform[] transform = GetComponentsInChildren<Transform>();

        foreach (Transform t in transform)
        {
            t.gameObject.layer = 0;
        }

        _isEquiped = false;
    }
    private void OnEnable()
    {
        _canShoot = true;
    }
}
