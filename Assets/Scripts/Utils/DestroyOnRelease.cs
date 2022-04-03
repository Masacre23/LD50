using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnRelease : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        #if UNITY_STANDALONE
            Destroy(this);
        #endif
    }

}
