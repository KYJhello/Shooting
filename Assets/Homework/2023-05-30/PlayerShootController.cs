using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootController : MonoBehaviour
{
    [SerializeField] Transform shootStartP;
    [SerializeField] private int damage;
    [SerializeField] private ParticleSystem muzzleEffect;

    private ParticleSystem bulletEffect;

    private void Awake()
    {
        
    }

    private void Fire()
    {
        
    }
    private void OnFire(InputValue value)
    {
        Fire();
    }
}
