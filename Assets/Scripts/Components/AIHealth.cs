using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health
{
    private AIController aIToDelete;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Heal(float amount)
    {
        currentHealth = currentHealth + amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (gameObject.GetComponent<UIManager>() != null)
        {
            gameObject.GetComponent<UIManager>().UpdateAIHealthUI();
        }
    }

    public override void IncreaseMaxHealth(float amount)
    {
        maxHealth = maxHealth + amount;

        if (gameObject.GetComponent<UIManager>() != null)
        {
            gameObject.GetComponent<UIManager>().UpdateAIHealthUI();
        }
    }

    public override void TakeDamage(float amount, Pawn source)
    {
        currentHealth = currentHealth - amount;

        // Makes sure first value stays somewhere between the second and third value
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);

        if (gameObject.GetComponent<UIManager>() != null)
        {
            gameObject.GetComponent<UIManager>().UpdateAIHealthUI();
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

                    ScoreManager.instance.UpdateCurrentScore();
                }
            }

            Die();
        }
    }

    public override void Die()
    {
        if (gameObject.GetComponent<AITankPawn>() != null)
        {
            foreach (AIController controller in GameManager.instance.aIPlayers)
            {
                //Debug.Log("Controller: " + controller + ", and its pawn: " + controller.pawn);
                //Debug.Log("The object trying to die: " + gameObject);

                if (controller.pawn == gameObject.GetComponent<AITankPawn>())
                {
                    aIToDelete = controller;
                }
            }

            if (aIToDelete != null)
            {
                AudioManager.instance.PlaySound("AI Tank Death", gameObject.transform);

                GameManager.instance.RemoveAIPlayerFromAIPlayers(aIToDelete);

                // Last AI on the map, spawn the next wave
                if (GameManager.instance.aIPlayers.Count <= 0)
                {
                    GameManager.instance.SpawnNewAIWave();
                }

                aIToDelete.Die();
                Destroy(gameObject);
            }
        }
    }
}