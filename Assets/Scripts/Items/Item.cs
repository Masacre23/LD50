using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    GASOIL,
    WOOD,
    HUMAN,
    DEFAULT
}

public class Item : MonoBehaviour
{
    public float illumination = 0f;
    public ItemType type;
    public Vector3 rotationReference;
    private bool burned = false;
    private void Start()
    {
        FindObjectOfType<GameManager>().worldItems++;
    }


    public void Burned()
    {
        burned = true;
        foreach (var item in gameObject.GetComponentsInChildren<Renderer>())
        {
            foreach (var m in item.materials)
            {
                m.SetColor("_HColor", Color.red);

            }

        }
        FindObjectOfType<GameManager>().worldItems--;
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        //  Debug.Log(Vector3.Distance(FindObjectOfType<PlayerController>().transform.position, transform.position));
        if (burned)
            return;
        if (Vector3.Distance(FindObjectOfType<PlayerController>().transform.position, transform.position) < 8f)
        {
            if (highlighting == null)
                highlighting = StartCoroutine(Highlighting());
        }
        else if (highlighting != null)
        {
            foreach (var item in gameObject.GetComponentsInChildren<Renderer>())
            {
                foreach (var m in item.materials)
                {
                    m.SetColor("_HColor", Color.white);

                }

            }

            StopCoroutine(highlighting);
            highlighting = null;
        }
    }

    Coroutine highlighting;
    IEnumerator Highlighting()
    {
        float t = Time.time;
        float tInter = 0;
        float tAnim = 1f;
        while (!burned)
        {
            t = Time.time;
            tInter = 0;
            while (Time.time - t < tAnim && !burned)
            {
                tInter = (Time.time - t) / tAnim;
                Color c = Color.Lerp(Color.white, Color.red, tInter);
                foreach (var item in gameObject.GetComponentsInChildren<Renderer>())
                {
                    foreach (var m in item.materials)
                    {
                        m.SetColor("_HColor", c);

                    }

                }
                yield return new WaitForFixedUpdate();
            }

            t = Time.time;
            tInter = 0;
            while (Time.time - t < tAnim && !burned)
            {
                tInter =((Time.time - t) / tAnim);
                Color c = Color.Lerp(Color.red, Color.white, tInter);
                foreach (var item in gameObject.GetComponentsInChildren<Renderer>())
                {
                    foreach (var m in item.materials)
                    {
                        m.SetColor("_HColor", c);

                    }

                }
                yield return new WaitForFixedUpdate();
            }
        }



    }


}