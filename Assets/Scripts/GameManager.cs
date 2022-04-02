using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float fadeTime = 2f;
    [SerializeField] Image fadePanel;
    [SerializeField] Image gameOverPopup;
    bool alreadyEnded = false;
    public async void GameOver()
    {
        if (alreadyEnded)
            return;

        alreadyEnded = true;
        fadePanel.Fade(fadeTime);
        await Task.Delay(2000);
        gameOverPopup.gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public async void WinAsync()
    {
        if (alreadyEnded)
            return;

        alreadyEnded = true;
        fadePanel.Fade(fadeTime);
        await Task.Delay(2000);
        SceneManager.LoadScene(1);
    }
}
