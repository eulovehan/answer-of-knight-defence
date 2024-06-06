using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nameStoreObject : MonoBehaviour
{
    public string playerName = "noname";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
