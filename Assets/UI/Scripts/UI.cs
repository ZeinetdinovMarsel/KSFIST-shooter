using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Image _healthImage;
    [SerializeField] private TMP_Text _currAmmo;
    [SerializeField] private TMP_Text _maxAmmo;
    [SerializeField] private TMP_Text _weaponName;
    [SerializeField] private GameObject _deathScreen;

    [SerializeField] private Health _health;
    [SerializeField] private WeaponManager _weaponManager;
    private Weapon _weapon;

    private void Start()
    {
        _health.OnHit += ChangeHealth;
        _health.OnDie += Die;
        _weaponManager.OnWeaponChanged += ChangeWeapon;
    }
    private void ChangeHealth()
    {
        _healthImage.fillAmount = _health.GetHealthParts();
    }

    private void ChangeWeapon()
    {
        _weapon = _weaponManager.CurrentWeapon;

        _maxAmmo.text = _weapon.MaxAmmo.ToString();
        _currAmmo.text = _weapon.CurrentAmmo.ToString();
        _weaponName.text = _weapon.WeaponName;

        _weapon.OnCurrAmmoChanged += ChangeCurrAmmo;
    }

    private void ChangeCurrAmmo()
    {
        _currAmmo.text = _weapon.CurrentAmmo.ToString();
    }

    private void Die()
    {
        _deathScreen.SetActive(true);
        Cursor.lockState= CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
}
