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
                aIToDelete.Die();
                Destroy(gameObject);
            }
        }
    }
}