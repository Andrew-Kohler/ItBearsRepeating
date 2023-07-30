using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    private bool _isGameOver;   // Has the player been defeated?
    private bool _isCutscene;   // Are we in a cutscene?
    private bool _isGameplay;   // Is gameplay going on?
    private bool _isLevelOver;  // Has the level concluded?

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject managerHolder = new GameObject("[Game Manager]");
                managerHolder.AddComponent<GameManager>();
            }

            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject); // Don't make me regret this
    }

    public void GameOver(bool flag) // Setter and getter for state of player defeat
    {
        _isGameOver = flag;
        if(flag == true)
        {
            onGameOver?.Invoke();
        }
    }
    public bool isGameOver()
    {
        return _isGameOver;
    }

    public void Gameplay(bool flag) // Setter and getter for state of gameplay
    {
        _isGameplay = flag;
    }
    public bool isGameplay()
    {
        return _isGameplay;
    }


}
