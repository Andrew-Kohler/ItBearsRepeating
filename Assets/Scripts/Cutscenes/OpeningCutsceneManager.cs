using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject firstSpeaker;
    private DialogueManager manager;

    private void OnEnable()
    {
        DialogueManager.onConvoOver += EndScene;
    }

    private void OnDisable()
    {
        DialogueManager.onConvoOver -= EndScene;
    }

    void Start()
    {
        manager = firstSpeaker.GetComponent<DialogueManager>();
        StartCoroutine(DoStartCutscene());
    }

    private void EndScene()
    {
        ViewManager.GetView<LevelStartView>().EndLevel(1);
    }

    IEnumerator DoStartCutscene()
    {
        yield return new WaitForSeconds(11f);
        firstSpeaker.SetActive(true);
        manager.ReadNextLine();
    }
}
