using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
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

        if (gameObject.GetComponent<UIManager>() != null)
        {
            gameObject.GetComponent<UIManager>().UpdateHealthUI();
        }

        if (currentHealth <= 0)
        {
            // Gives points to the player that dealt this object the final blow
            foreach (PlayerController controller in ScoreManager.instance.players)
            {
                if (controller.pawn == source && gameObject.GetComponent<PointGiver>() != null)
                {
                    controller.pawn.AddPoints(gameObject.GetComponent<PointGiver>().pointsOnDeath);

                    controller.pawn.GetComponent<UIManager>().UpdateScoreUI();
                }
            }

            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        currentHealth = currentHealth + amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (gameObject.GetComponent<UIManager>() != null)
        {
            gameObject.GetComponent<UIManager>().UpdateHealthUI();
        }
    }

    public virtual void IncreaseMaxHealth(float amount)
    {
        maxHealth = maxHealth + amount;

        if (gameObject.GetComponent<UIManager>() != null)
        {
            gameObject.GetComponent<UIManager>().UpdateHealthUI();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}