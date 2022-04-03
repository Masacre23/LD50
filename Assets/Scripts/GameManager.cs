using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float fadeTime = 2f;
    [SerializeField] Image fadeDarkPanel;
    [SerializeField] Image fadeWhitePanel;
    [SerializeField] Image fadeGreenPanel;
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
    private int _worldNPCs;
    public int worldNPCs
    {
        get { return _worldNPCs; }
        set
        {
            _worldNPCs = value;
            Debug.Log("NPCS IN THE WORLD: " + _worldNPCs);

            if (_worldNPCs == 0)
            {
                Debug.Log("GAME OVER");
                FindObjectOfType<GameManager>().WinAsync();
            }
        }
    }

    public async void GameOver()
    {
        if (alreadyEnded)
            return;

        GameObject.Find("GlobalEffects").GetComponent<AudioSource>().PlayOneShot(Resources.Load("Audios/death") as AudioClip);
        alreadyEnded = true;
        fadeDarkPanel.Fade(fadeTime);
        await Task.Delay(2000);
        gameOverPopup.gameObject.SetActive(true);
        await Task.Delay(5000);
        SceneManager.LoadScene(1);
    }

    public async void WinAsync()
    {
        if (alreadyEnded)
            return;

        alreadyEnded = true;
        foreach (var audio in GameObject.Find("Screams").GetComponents<AudioSource>())
        {
            audio.loop = false;
        }

        //Fade verde
        fadeGreenPanel.Fade(0.5f);
        await Task.Delay(500);
        foreach(var monster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            monster.SetActive(false);
        }
        //Pasamos de verde a blanco
        fadeWhitePanel.Fade(0);
        fadeGreenPanel.Unfade(0.5f);
        await Task.Delay(500);

        //Mostramos la escena final
        winGameObject.SetActive(true);
        var wincanvas = winGameObject.transform.GetChild(0).gameObject;
        wincanvas.SetActive(false);
        fadeWhitePanel.Unfade(fadeTime);
        await Task.Delay(2000);

        //Habilitamos el texto
        wincanvas.SetActive(true);
        await Task.Delay(10000);

        SceneManager.LoadScene(1);
    }
}
