using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayShootAnimation()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Shoot");
        }
    }
}
