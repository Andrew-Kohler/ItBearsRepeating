using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject creditsSubmenu;

    private void Start()
    {
        creditsSubmenu.SetActive(false);
    }
    public void StartButton()
    {
        GameManager.Instance.Gameplay(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditsOpenButton()
    {
        StopAllCoroutines();
        StartCoroutine(DoFadeInCredits());
    }

    public void CreditsCloseButton()
    {
        StopAllCoroutines();
        StartCoroutine(DoFadeOutCredits());
    }

    IEnumerator DoFadeInCredits()
    {
        creditsSubmenu.SetActive(true);
        CanvasGroup canvasGroup = creditsSubmenu.GetComponent<CanvasGroup>();

        creditsSubmenu.GetComponentInChildren<Animator>().Play("Scroll", 0, 0);
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * 3f;
            yield return null;
        }
        //creditsSubmenu.GetComponentInChildren<Animator>().Play("Scroll", 0, 0);
        yield return null;

    }

    IEnumerator DoFadeOutCredits()
    {
        CanvasGroup canvasGroup = creditsSubmenu.GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * 3f;
            yield return null;
        }
        creditsSubmenu.SetActive(false);
        yield return null;
    }
}
