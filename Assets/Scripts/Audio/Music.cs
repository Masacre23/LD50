using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update() {
        if (Camera.main != null)
            this.transform.position = Camera.main.transform.position;
    }
}
