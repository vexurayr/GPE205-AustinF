using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Reference to itself to reach objects anywhere in the heiarchy
    public static UIManager instance;

    // For UI
    [SerializeField] public Text cooldownText;
    [SerializeField] public Text healthText;

    private void Awake()
    {
        // Only allows for one game manager, one singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
