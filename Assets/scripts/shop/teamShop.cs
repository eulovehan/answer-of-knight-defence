using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class teamStop : MonoBehaviour
{
    public GameObject player;
    public GameObject sideTextUI;
    public GameObject teamCreate;
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

        // teamCreate 찾기
        GameObject[] findTeamCreate = GameObject.FindGameObjectsWithTag("TeamCreate");
        foreach (GameObject find in findTeamCreate)
        {
            teamCreate = find;
        }
    }

    void Update() {
        int coin = player.GetComponent<player>().coin;
        // e를 누르면 팀 생성
        if (Input.GetKeyDown(KeyCode.E) && inTrigger && coin >= 700)
        {
            // 금액 차감
            player.GetComponent<player>().coin -= 700;
            
            // 팀 생성
            teamCreate.GetComponent<teamCreate>().CreateTeam();
        }
    }

    // 플레이어랑 부딪히면 텍스트 출력
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            sideTextUI.GetComponent<text>().TextViewFix("팀 생성 (700 coin) (E)");
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
