using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerController playerToDelete;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Die()
    {
        if (gameObject.GetComponent<PlayerTankPawn>() != null)
        {
            foreach (PlayerController controller in GameManager.instance.players)
            {
                if (controller.pawn == gameObject.GetComponent<PlayerTankPawn>())
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
    }
}