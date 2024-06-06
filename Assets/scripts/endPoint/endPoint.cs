using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class endPoint : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 찾기
        GameObject[] findPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject find in findPlayer)
        {
            player = find;
        }
    }
    
    // 적이랑 부딪히면 게임오버 (enemy tag)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            player.GetComponent<player>().Death(true);
        }
    }
}
