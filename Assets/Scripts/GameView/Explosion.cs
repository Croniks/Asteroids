using System;

using UnityEngine;


public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;

    private Action _actionAfterExplosion;

    private void Awake()
    {
        var explosionMainModule = _explosion.main;
        explosionMainModule.stopAction = ParticleSystemStopAction.Callback;
    }

    public void DoExplosion(Action afterAction)
    {
        _actionAfterExplosion = afterAction;
        _explosion.Play();
    }

    private void OnParticleSystemStopped()
    {
        _actionAfterExplosion?.Invoke();
        _actionAfterExplosion = null;
    }
}