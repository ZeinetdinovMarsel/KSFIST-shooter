using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;

    private Ragdoll _ragdoll;
    public float CurrentHealth => _currentHealth;
    public bool IsAlive => _currentHealth > 0;

    public event Action OnHit;
    public event Action OnDie;
    private void Start()
    {
        _currentHealth = _maxHealth;

        _ragdoll = GetComponentInChildren<Ragdoll>();
        _ragdoll.DisableRagdoll();

        foreach (var rb in _ragdoll.rbs)
        {
            if (rb.gameObject.layer != 3)
            {
                HitBox hb = rb.gameObject.AddComponent<HitBox>();
                hb.Health = this;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;

        OnHit?.Invoke();
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (!IsAlive) return;

        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }

    private void Die()
    {
        OnDie?.Invoke();
        _ragdoll?.EnableRagdoll();
        this.enabled = false;
    }
}
