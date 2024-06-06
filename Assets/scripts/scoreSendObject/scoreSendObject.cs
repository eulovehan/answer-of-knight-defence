using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreSendObject : MonoBehaviour
{
    public int score = 0;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (player) {
            score = player.GetComponent<player>().score;
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
