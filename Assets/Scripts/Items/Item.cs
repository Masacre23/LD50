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
        if (GetComponent<Outline>())
            GetComponent<Outline>().OutlineWidth = 0f;
        foreach (var item in GetComponentsInChildren<Outline>())
        {
            item.OutlineWidth = 0f;
        }
        FindObjectOfType<GameManager>().worldItems++;
    }


    public void Burned()
    {
        burned = true;
        if (GetComponent<Outline>())
            GetComponent<Outline>().OutlineWidth = 0f;
        foreach (var item in GetComponentsInChildren<Outline>())
        {
            item.OutlineWidth = 0f;
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
            if (GetComponent<Outline>())
                GetComponent<Outline>().OutlineWidth = 0f;
            foreach (var item in GetComponentsInChildren<Outline>())
            {
                item.OutlineWidth = 0f;
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

                if (GetComponent<Outline>())
                    GetComponent<Outline>().OutlineWidth = tInter * 3f;
                foreach (var item in GetComponentsInChildren<Outline>())
                {
                    item.OutlineWidth = tInter * 3f;
                }
                yield return new WaitForFixedUpdate();
            }

            t = Time.time;
            tInter = 0;
            while (Time.time - t < tAnim && !burned)
            {
                tInter = 1 - ((Time.time - t) / tAnim);


                if (GetComponent<Outline>())
                    GetComponent<Outline>().OutlineWidth = tInter * 3f;
                foreach (var item in GetComponentsInChildren<Outline>())
                {
                    item.OutlineWidth = tInter * 3f;
                }
                yield return new WaitForFixedUpdate();
            }
        }



    }


}