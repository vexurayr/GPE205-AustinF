using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    private PlayerController playerToDelete;
    private AIController aIToDelete;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float amount, Pawn source)
    {
        currentHealth = currentHealth - amount;
        // Makes sure first value stays somewhere between the second and third value
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = currentHealth + amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth = maxHealth + amount;
    }

    public void Die()
    {
        if (gameObject.GetComponent<TankPawn>() != null &&
            gameObject.GetComponent<TankPawn>().isPlayer == true)
        {
            foreach (PlayerController controller in GameManager.instance.players)
            {
                if (controller.pawn == gameObject.GetComponent<TankPawn>())
                {
                    playerToDelete = controller;
                }
            }

            // Removes this object from its list in the game manager
            if (playerToDelete != null)
            {
                GameManager.instance.RemovePlayerFromPlayers(playerToDelete);
                playerToDelete.Die();
                Destroy(gameObject);
            }
        }
        else if (gameObject.GetComponent<TankPawn>() != null &&
            gameObject.GetComponent<TankPawn>().isPlayer == false)
        {
            foreach (AIController controller in GameManager.instance.aIPlayers)
            {
                //Debug.Log("Controller: " + controller + ", and its pawn: " + controller.pawn);
                //Debug.Log("The object trying to die: " + gameObject);

                if (controller.pawn == gameObject.GetComponent<TankPawn>())
                {
                    aIToDelete = controller;
                }
            }

            if (aIToDelete != null)
            {
                GameManager.instance.RemoveAIPlayerFromAIPlayers(aIToDelete);
                aIToDelete.Die();
                Destroy(gameObject);
            }
        }
        else if (gameObject.GetComponent<Health>() != null)
        {
            Destroy(gameObject);
        }
    }
}