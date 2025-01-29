using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private KeyCode _pickupKey = KeyCode.E;

    [Header("Settings")]
    [SerializeField] private List<Weapon> _weapons;
    public Weapon CurrentWeapon;
    private int _currentWeaponIndex = -1;

    public event Action OnWeaponChanged;
    private void Start()
    {
        EquipWeapon(0);
    }

    private void Update()
    {
        HandleWeaponSwitch();
        HandlePickup();
    }

    private void HandleWeaponSwitch()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && i < _weapons.Count)
            {
                EquipWeapon(i);
                break;
            }
        }
    }

    private void HandlePickup()
    {
        if (Input.GetKeyDown(_pickupKey))
        {
            Vector3 cameraPoint = new Vector3(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(cameraPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                Weapon weapon = hit.collider.GetComponent<Weapon>();
                if (weapon != null)
                {
                    EquipWeapon(weapon);
                }
            }
        }
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= _weapons.Count || _weapons[index] == null) return;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.gameObject.SetActive(false);
        }

        _currentWeaponIndex = index;
        CurrentWeapon = _weapons[_currentWeaponIndex];
        CurrentWeapon.gameObject.SetActive(true);
        OnWeaponChanged?.Invoke();
    }

    private void EquipWeapon(Weapon weapon)
    {
        if (weapon.IsEquiped)
        {
            return;
        }

        if (CurrentWeapon != null)
        {
            CurrentWeapon.gameObject.SetActive(false);
        }

        CurrentWeapon = weapon;
        _weapons.Add(weapon);
        CurrentWeapon.EquipWeapon(_weaponHolder);
        CurrentWeapon.gameObject.SetActive(true);
        OnWeaponChanged?.Invoke();
    }
}
