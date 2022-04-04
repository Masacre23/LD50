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
    [SerializeField] GameObject dialogText;
    [SerializeField] GameObject firePointLight;
    Fireplace fireplace;
    bool alreadyEnded = false;

    private int _worldItems;
    public int worldItems
    {
        get { return _worldItems; }
        set
        {
            if (value < _worldItems)
                Debug.Log("ITEMS IN THE WORLD: " + (_worldItems-1));

            //if (value == 12)
              //  panicPhaseOn = true;
            _worldItems = value;

        }
    }

    public bool panicPhaseOn;
    private int _worldNPCs;
    public int worldNPCs
    {
        get { return _worldNPCs; }
        set
        {
            if (value < _worldNPCs)
            {
                panicPhaseOn = true;

                Debug.Log("NPCS IN THE WORLD: " + (_worldNPCs-1));

            }
            _worldNPCs = value;

            if (_worldNPCs == 0)
            {
                Debug.Log("GAME OVER");
                FindObjectOfType<GameManager>().WinAsync();
            }
        }
    }

    async void Start()
    {
        fireplace = FindObjectOfType<Fireplace>();
        await Task.Delay(10000);
        dialogText.SetActive(false);
    }

    private void Update()
    {
            panicPhaseOn = fireplace.power < 20 && worldItems <= 12;
    }

    public async void GameOver()
    {
        if (alreadyEnded)
            return;

        var player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponentInChildren<Animator>().SetBool("Dying", true);
        if(GameObject.Find("GlobalEffects"))
        GameObject.Find("GlobalEffects").GetComponent<AudioSource>().PlayOneShot(Resources.Load("Audios/death") as AudioClip);
        alreadyEnded = true;
        fadeDarkPanel.Fade(fadeTime);
        await Task.Delay(2000);
        FindObjectOfType<Fireplace>().ChangeFireColor(ItemType.WOOD);
        gameOverPopup.gameObject.SetActive(true);
        await Task.Delay(5000);
        SceneManager.LoadScene(1);
    }

    public async void WinAsync()
    {
        if (alreadyEnded)
            return;

        var player = GameObject.Find("Player");
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponentInChildren<Animator>().SetFloat("Speed", 0f);
        alreadyEnded = true;
        foreach (var audio in GameObject.Find("Screams").GetComponents<AudioSource>())
        {
            audio.loop = false;
        }

        //Fade verde
        fadeGreenPanel.Fade(0.5f);
        await Task.Delay(500);
        firePointLight.SetActive(false);
        foreach (var monster in GameObject.FindGameObjectsWithTag("Monster"))
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
