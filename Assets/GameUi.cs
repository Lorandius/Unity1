using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUi : MonoBehaviour
{
    public GameObject gameLoseUI; 
    public GameObject gameWinUI;

    private bool gameIsOver;
    // Start is called before the first frame update
    void Start()
    {
        Guard.OnGuardHasSpottenPlayer += showGamesLoseUI;
        PlayerMove.OnReachedEndOfLevel += showGamesWinUI;
        // FindObjectOfType<PlayerMove>().OnReachedEndOfLevel += showGamesWinUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }   
    }

    void showGamesWinUI()
    {
        onGameO(gameWinUI);

    }

    void showGamesLoseUI()
    {
        onGameO(gameLoseUI);
    }
    void onGameO(GameObject gameStage)
    {
        gameStage.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottenPlayer -= showGamesLoseUI;
        PlayerMove.OnReachedEndOfLevel -= showGamesWinUI;


    }
}
