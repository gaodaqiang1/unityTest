using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(StartGame);
    }
    private void StartGame()
    {
       // SceneManager.LoadScene("GamePlay");
       TransitioniManager.instance.Transition("GamePlay");
    }

}
