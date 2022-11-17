using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    // Start is called before the first frame update
    public virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual float GetHealth()
    {
        return currentHealth;
    }

    public virtual void TakeDamage(float amount, Pawn source)
    {
        currentHealth = currentHealth - amount;
        // Makes sure first value stays somewhere between the second and third value
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);

        if (GetComponentInParent<UIManager>() != null)
        {
            GetComponentInParent<UIManager>().UpdateHealthUI();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        currentHealth = currentHealth + amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (GetComponentInParent<UIManager>() != null)
        {
            GetComponentInParent<UIManager>().UpdateHealthUI();
        }
    }

    public virtual void IncreaseMaxHealth(float amount)
    {
        maxHealth = maxHealth + amount;

        if (GetComponentInParent<UIManager>() != null)
        {
            GetComponentInParent<UIManager>().UpdateHealthUI();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}