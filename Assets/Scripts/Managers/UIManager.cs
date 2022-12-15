using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Anything that needs UI will have a UIManager
public class UIManager : MonoBehaviour
{
    public PlayerTankPawn pawn;

    // Will change the Y scale 0-1
    [SerializeField] public Image shootCooldownForeground;

    // Here to change the image that displays the player's ammo
    [SerializeField] public RawImage shootCooldown;

    // Will change the X scale 0-1
    [SerializeField] public Image health;

    // Health bar will change width if max health increases
    [SerializeField] public Image healthBackground;

    // Changes the UI so the player knows what projectile they'll shoot next
    [SerializeField] public Texture PlayerAmmoBackground;
    [SerializeField] public Texture StunAmmoBackground;

    // Duct Tape are the number of lives the player have, like the number of times they can be repaired
    [SerializeField] public RawImage ductTape1;
    [SerializeField] public RawImage ductTape2;
    [SerializeField] public RawImage ductTape3;

    // Points from destroying tanks and buildings increases the score
    [SerializeField] public Text score;

    // For AI UI
    public AITankPawn aIPawn;
    public Canvas aICanvas;
    public Image aIHealth;

    private float startMaxHealth;
    private float startHealthWidth;

    public void Start()
    {
        if (aIPawn != null)
        {
            startMaxHealth = aIPawn.GetComponent<AIHealth>().maxHealth;
            startHealthWidth = aIHealth.rectTransform.rect.width;
        }
        else if (pawn != null)
        {
            startMaxHealth = pawn.GetComponent<Health>().maxHealth;
            startHealthWidth = health.rectTransform.rect.width;
        }
    }

    public void Update()
    {
        if (aIPawn != null && GameManager.instance.players.Count > 0)
        {
            MakeAICanvasFacePlayerCamera();
        }
    }

    public void UpdateHealthUI()
    {
        // Bail if there is no health component
        if (pawn.GetComponent<Health>() == null)
        {
            return;
        }

        float currentHealth = pawn.GetComponent<Health>().GetHealth();
        float maxHealth = pawn.GetComponent<Health>().maxHealth;

        // Calculate new width
        float currentHealthWidth = startHealthWidth + (maxHealth - startMaxHealth);

        // Set new width
        health.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHealthWidth);
        healthBackground.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHealthWidth);

        // Calculate the scale of the health bar
        float healthScaleX = currentHealth / maxHealth;

        // Set health bar scale
        health.GetComponent<RectTransform>().localScale = new Vector3(healthScaleX, 1, 1);
    }

    public void UpdateShootCooldownUI()
    {
        // Bail if there is no player tank pawn component
        if (pawn.GetComponent<PlayerTankPawn>() == null)
        {
            return;
        }

        float secondsPerShot = GetComponentInParent<PlayerTankPawn>().GetSecondsPerShot();
        float cooldown = GetComponentInParent<PlayerTankPawn>().GetTimeUntilNextEvent();
        
        // Calculate the scale of the shoot cooldown foreground
        float cooldownScaleY = cooldown / secondsPerShot;

        // Set shoot cooldown foreground scale
        shootCooldownForeground.GetComponent<RectTransform>().localScale = new Vector3(1, cooldownScaleY, 1);

        if (pawn.GetComponent<PowerupManager>() != null)
        {
            if (pawn.GetComponent<PowerupManager>().HasStunPowerup())
            {
                shootCooldown.GetComponent<RawImage>().texture = StunAmmoBackground;
            }
            else
            {
                shootCooldown.GetComponent<RawImage>().texture = PlayerAmmoBackground;
            }
        }
    }

    public void UpdateScoreUI()
    {
        // Bail if there is no player tank pawn component
        if (pawn.GetComponent<PlayerTankPawn>() == null)
        {
            return;
        }

        int currentScore = pawn.GetScore();

        score.text = "Score: " + currentScore;
    }

    public void UpdateLivesUI()
    {
        int livesRemaining = LivesManager.instance.lives;

        if (livesRemaining == 0)
        {
            ductTape1.enabled = false;
            ductTape2.enabled = false;
            ductTape3.enabled = false;
        }
        else if (livesRemaining == 1)
        {
            ductTape1.enabled = true;
            ductTape2.enabled = false;
            ductTape3.enabled = false;
        }
        else if (livesRemaining == 2)
        {
            ductTape1.enabled = true;
            ductTape2.enabled = true;
            ductTape3.enabled = false;
        }
        else if (livesRemaining == 3)
        {
            ductTape1.enabled = true;
            ductTape2.enabled = true;
            ductTape3.enabled = true;
        }
    }

    // Only for AI tanks, health bar will not change size when max health increases
    public void UpdateAIHealthUI()
    {
        // Bail if there is no health component
        if (aIPawn.GetComponent<AIHealth>() == null)
        {
            return;
        }

        float currentHealth = aIPawn.GetComponent<AIHealth>().GetHealth();
        float maxHealth = aIPawn.GetComponent<AIHealth>().maxHealth;

        // Calculate the scale of the health bar
        float healthScaleX = currentHealth / maxHealth;

        // Set health bar scale
        aIHealth.GetComponent<RectTransform>().localScale = new Vector3(healthScaleX, 1, 1);
    }

    public void MakeAICanvasFacePlayerCamera()
    {
        aICanvas.transform.rotation = new Quaternion(GameManager.instance.players[0].pawn.camera.transform.rotation.x,
            GameManager.instance.players[0].pawn.camera.transform.rotation.y,
            GameManager.instance.players[0].pawn.camera.transform.rotation.z,
            GameManager.instance.players[0].pawn.camera.transform.rotation.w);
    }
}