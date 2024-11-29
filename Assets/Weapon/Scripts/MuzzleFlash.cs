using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    private ParticleSystem _flashEffect;

    private void Start()
    {
        _flashEffect = GetComponent<ParticleSystem>();
    }
    public void PlayFlash()
    {
        if (_flashEffect != null)
        {
            _flashEffect.Play();
        }
    }

    public void StopFlash()
    {
        if (_flashEffect != null)
        {
            _flashEffect.Stop();
        }
    }

}
