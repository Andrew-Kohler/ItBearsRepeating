using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InGameUIView : View
{
    [SerializeField] private MoveStatWheel healthMove; // The move scripts for each wheel
    [SerializeField] private MoveStatWheel dashMove;

    [SerializeField] private Slider healthSlider;   // The slider components of each meter
    [SerializeField] private Slider dashSlider;
    [SerializeField] private Image dashFill;
    

    [SerializeField] private GameObject player;
    private Health playerHealth;
    private PlayerSprint playerSprint;
    private PlayerMovement playerMovement;

    private bool dashOnscreen;
    private bool dashRecharge;

    public override void Initialize()
    {
        playerHealth = player.GetComponent<Health>();
        playerSprint = player.GetComponent<PlayerSprint>(); 
        playerMovement = player.GetComponent <PlayerMovement>();

        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.currentHealth;

        dashSlider.maxValue = playerSprint.maxStamina;
        dashSlider.value = playerSprint.stamina;

        dashOnscreen = false;
        dashRecharge = false;

    }

    private void OnEnable()
    {
        Health.onPlayerDamage += ActivateHealthWheel;
        GameManager.onGameOver += SwitchToGameOver;
    }

    private void OnDisable()
    {
        Health.onPlayerDamage -= ActivateHealthWheel;
        GameManager.onGameOver -= SwitchToGameOver;
    }

    void Start()
    {
    }

// Update is called once per frame
    void Update()
    {
        UpdatePlayerStats();
        UpdateDashWheel();
    }

    private void UpdatePlayerStats()
    {
        healthSlider.value = playerHealth.currentHealth;
        dashSlider.value = playerSprint.stamina;
    }

    private void UpdateDashWheel()
    {
        if (!dashRecharge) 
        {
            if (playerMovement.IsSprinting && !dashOnscreen && playerMovement.HMovement != 0)
            {
                dashMove.MoveOnscreen();
                dashOnscreen = true;
            }
            else if ((!playerMovement.IsSprinting && dashOnscreen) || (playerMovement.HMovement == 0 && dashOnscreen))
            {
                dashMove.MoveOffscreen();
                dashOnscreen = false;
            }

            if (!playerSprint.canSprint)
            {
                StartCoroutine(DoDashWheelRecharge());
            }
        }
        
    }

    public void ActivateHealthWheel()
    {
        StartCoroutine(DoMoveHealthWheel());
    }

    private void SwitchToGameOver()
    {
        ViewManager.ShowFade<GameOverView>(false);
    }

    IEnumerator DoMoveHealthWheel()
    {
        healthMove.MoveOnscreen();
        yield return new WaitForSeconds(3f);
        if(playerHealth.currentHealth > (playerHealth.maxHealth / 2))   // If the player's health drops below 50, we want the HUD to stay
        {
            healthMove.MoveOffscreen();
        }
        yield return null;
    }

    IEnumerator DoDashWheelRecharge()
    {
        float colorStep = .8f;

        dashRecharge = true;
        dashMove.MoveOnscreen();
        Debug.Log(dashFill.color.b);
        while (!playerSprint.canSprint)
        {
            dashFill.color = new Color(dashFill.color.r - (colorStep * Time.deltaTime), dashFill.color.g - (colorStep * Time.deltaTime), dashFill.color.b);
            
            if(dashFill.color.g < .1f)
            {
                dashFill.color = Color.white;
                yield return new WaitForSeconds(.3f);
            }
            yield return null;
        }
        dashFill.color = Color.blue;
        dashRecharge = false;
        yield return null;
    }

    // While (!canSprint)
    //  move towards blue
    //  if we are a certain amount close to blue, set color to white
    // set color to blue
}
