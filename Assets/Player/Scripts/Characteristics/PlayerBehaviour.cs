using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private WeaponManager _weaponManager;

    private Health _health;
    private void Start()
    {
        _health = GetComponent<Health>();
        _health.OnDie += Die;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        _virtualCamera.gameObject.SetActive(false);
        _weaponManager.gameObject.SetActive(false);
    }
}
