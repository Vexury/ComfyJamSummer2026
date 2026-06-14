using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;
    [SerializeField] private float damageCooldown = 0.5f;

    public static event Action OnDeath;
    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    private int currentHP;
    private float lastDamageTime = float.MinValue;

    private void Awake()
    {
        currentHP = maxHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;
        if (Time.time - lastDamageTime <= damageCooldown) return;

        lastDamageTime = Time.time;
        currentHP--;

        if (currentHP <= 0)
            OnDeath?.Invoke();
    }
}
