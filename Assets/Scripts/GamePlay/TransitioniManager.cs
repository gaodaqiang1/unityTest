using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class TransitioniManager : MonoBehaviour
{
    public static TransitioniManager instance;
    private CanvasGroup canvasGroup;
    public float scaler;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (instance == null)
            instance = this;
        else

            Destroy(this.gameObject);


        DontDestroyOnLoad(this);

    }

    private void Start()
    {
        StartCoroutine(Fade(0));
    }

    public void Transition(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        yield return Fade(1);
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return Fade(0);
    }
    /// <summary>
    /// 渐变
    /// </summary>
    /// <param name="amount">1是黑，0是透明</param>
    /// <returns></returns>
    private IEnumerator Fade(int amount)
    {
        canvasGroup.blocksRaycasts = true;

        while (canvasGroup.alpha != amount)
        {
            switch (amount)
            {
                case 1:
                    canvasGroup.alpha += Time.deltaTime * scaler;
                    break;
                case 0:
                    canvasGroup.alpha -= Time.deltaTime * scaler;
                    break;

            }
            yield return null;
        }


        canvasGroup.blocksRaycasts = false;

    }

}
