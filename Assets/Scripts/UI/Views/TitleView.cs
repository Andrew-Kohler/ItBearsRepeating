using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleView : View
{
    [SerializeField] GameObject creditsSubmenu;
    [SerializeField] PlayerMovement player;
    [SerializeField] View transitionTo;

    private bool closing;
    public override void Initialize()
    {
        //throw new System.NotImplementedException();
        closing = false;
    }

    private void Update()
    {
        if (GameManager.Instance.isGameplay() && !closing) // Once the start button has been clicked, fade the titles out and show the in-game UI
        {
            closing = true;
            player.DisablePlayer(false);
            player.hitstun = false;
            GetComponent<FadeUI>().UIFadeOut(transitionTo);
        }
    }

    

}
