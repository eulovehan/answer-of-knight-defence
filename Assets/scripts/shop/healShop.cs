using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class healStop : MonoBehaviour
{
    public GameObject player;
    public GameObject sideTextUI;
    private bool inTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 찾기
        GameObject[] findPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject find in findPlayer)
        {
            player = find;
        }

        // text ui set
        GameObject[] findUI = GameObject.FindGameObjectsWithTag("sideTextUI");
        foreach (GameObject ui in findUI)
        {
            sideTextUI = ui;
        }
    }

    void Update() {
        int coin = player.GetComponent<player>().coin;
        // e를 누르면 체력 회복
        if (Input.GetKeyDown(KeyCode.E) && inTrigger && coin >= 200)
        {
            // 금액 차감
            player.GetComponent<player>().coin -= 200;
            
            // 체력 회복
            player.GetComponent<player>().hp = player.GetComponent<player>().totalHp;
        }
    }

    // 플레이어랑 부딪히면 텍스트 출력
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            sideTextUI.GetComponent<text>().TextViewFix("체력회복 (200 coin) (E)");
            inTrigger = true;
        }
    }

    // 플레이어가 나가면 텍스트 삭제
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            sideTextUI.GetComponent<text>().TextViewFixEnd();
            inTrigger = false;
        }
    }
}
