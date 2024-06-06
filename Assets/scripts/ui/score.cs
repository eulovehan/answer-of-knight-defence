using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class score : MonoBehaviour
{
    public TextMeshProUGUI scoreContentText;  // UI 텍스트 요소
    public GameObject Player;  // 플레이어 게임 오브젝트

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreContentText.text = "score: " + Player.GetComponent<player>().score.ToString();  // UI 텍스트 업데이트
    }
}
