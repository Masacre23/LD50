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
    [SerializeField] GameObject winGameObject;
    bool alreadyEnded = false;

    private int _worldItems;
    public int worldItems
    {
        get { return _worldItems; }
        set { _worldItems = value;
            Debug.Log("ITEMS IN THE WORLD: " + _worldItems);

            if (_worldItems == 0)
            {
                Debug.Log("FINAL PHASE");
            }
        }
    }

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
        winGameObject.SetActive(true);
        fadePanel.Unfade(fadeTime);
        await Task.Delay(10000);
        SceneManager.LoadScene(1);
    }
}
