using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private string _weaponName;
    [SerializeField] private int _maxAmmo = 30;
    [SerializeField] private float _damage = 10f;
    public float FireRate = 0.2f;

    [Header("Shooting Settings")]
    public Transform FirePoint;
    [SerializeField] private float _range = 50f;

    [Header("Effects")]
    [SerializeField] ObjectPool _bulletTrailPool;
    [SerializeField] ObjectPool _hitDecalPool;
    [SerializeField] MuzzleFlash _muzzleFlash;


    private int _currentAmmo;
    private bool _canShoot = true;

    private void Awake()
    {
        _currentAmmo = _maxAmmo;
    }

    public string WeaponName => _weaponName;
    public int CurrentAmmo => _currentAmmo;
    public int MaxAmmo => _maxAmmo;
    public float Range => _range;

    public void Update()
    {
        if (Input.GetButtonUp("Fire1") && _muzzleFlash != null || (!_canShoot || _currentAmmo <= 0))
        {
            _muzzleFlash?.StopFlash();
        }

        if (Input.GetButtonDown("Reload"))
        {
            _currentAmmo = _maxAmmo;
        }
    }
    public void Shoot(Vector3 position, Vector3 direction)
    {
        if (!_canShoot || _currentAmmo <= 0) return;

        _currentAmmo--;
        PerformRaycast(position, direction);

        _muzzleFlash?.PlayFlash();

        StartCoroutine(FireCooldown());
    }

    private void PerformRaycast(Vector3 position, Vector3 direction)
    {
        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        Vector3 endPoint = position + direction * _range;

        if (Physics.Raycast(ray, out hit, _range))
        {

            Health targetHealth = hit.collider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(_damage);
            }

            GameObject decal = _hitDecalPool.GetObject();
            decal.GetComponent<HitDecal>()?.Initialize(hit.point, hit.normal, _hitDecalPool, hit.transform);

            endPoint = hit.point;
        }

        GameObject trail = _bulletTrailPool.GetObject();
        trail.transform.position = FirePoint.position;
        trail.transform.rotation = Quaternion.LookRotation(direction);
        trail.GetComponent<BulletTrail>()?.Initialize(FirePoint.position, endPoint);
    }


    private System.Collections.IEnumerator FireCooldown()
    {

        _canShoot = false;
        yield return new WaitForSeconds(FireRate);
        _canShoot = true;
    }
}
