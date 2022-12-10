using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerController controllerToDelete;
    private PlayerGamepadController gamepadToDelete;

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
                    controllerToDelete = controller;
                }
            }

            foreach (PlayerGamepadController controller in GameManager.instance.gamepadPlayers)
            {
                if (controller.pawn == gameObject.GetComponent<PlayerTankPawn>())
                {
                    gamepadToDelete = controller;
                }
            }

            // Removes this object from its list in the game manager
            if (controllerToDelete != null)
            {
                AudioManager.instance.PlaySound("Player Tank Death", gameObject.transform);

                GameManager.instance.RemovePlayerFromPlayers(controllerToDelete);

                controllerToDelete.Die();

                if (GameManager.instance.players.Count <= 0)
                {
                    Camera.main.GetComponent<AudioListener>().enabled = true;
                }

                Destroy(gameObject);
            }
            else if (gamepadToDelete != null)
            {
                AudioManager.instance.PlaySound("Player Tank Death", gameObject.transform);

                GameManager.instance.RemovePlayerFromGamepadPlayers(gamepadToDelete);

                gamepadToDelete.Die();

                if (GameManager.instance.players.Count <= 0)
                {
                    Camera.main.GetComponent<AudioListener>().enabled = true;
                }

                Destroy(gameObject);
            }
        }

        // Removes itself from the score manager
        if (ScoreManager.instance != null && ScoreManager.instance.players != null)
        {
            ScoreManager.instance.players.Remove(gameObject.GetComponent<PlayerController>());
        }
    }
}