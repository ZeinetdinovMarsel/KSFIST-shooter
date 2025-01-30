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
    [SerializeField] private GameObject _winScreen;

    [SerializeField] private Health _playerHealth;
    [SerializeField] private Health _enemyHealth;
    [SerializeField] private WeaponManager _weaponManager;
    private Weapon _weapon;

    private void Start()
    {
        _playerHealth.OnHit += ChangeHealth;
        _playerHealth.OnDie += Lose;
        _enemyHealth.OnDie += Win;
        _weaponManager.OnWeaponChanged += ChangeWeapon;
    }
    private void ChangeHealth()
    {
        _healthImage.fillAmount = _playerHealth.GetHealthParts();
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

    private void Win()
    {
        _winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    private void Lose()
    {
        _deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
}
