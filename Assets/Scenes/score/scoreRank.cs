using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

// 플레이어 데이터 클래스
[System.Serializable]
public class PlayerData
{
    public string name;
    public int score;
    public string date;
}

// 플레이어 데이터 리스트 클래스
[System.Serializable]
public class PlayerDataList
{
    public List<PlayerData> players;
}

public class scoreRank : MonoBehaviour
{
    public TextMeshProUGUI scoreContentText;  // UI 텍스트 요소
    private string filePath = "./score.json";  // 파일 경로

    // Start is called before the first frame update
    void Start()
    {
        // score.json 파일에서 데이터 로드
        List<PlayerData> allPlayerData = LoadPlayerData();
        if (allPlayerData != null)
        {
            // 내림차순으로 정렬
            allPlayerData.Sort((x, y) => y.score.CompareTo(x.score));
            
            // 최대 15개까지만 출력
            int count = 0;
            foreach (PlayerData playerData in allPlayerData)
            {
                scoreContentText.text += playerData.name + " - " + playerData.score + " point / [" + playerData.date + "]" + "\n";

                count++;
                if (count >= 15)
                {
                    break;
                }
            }
        }
    }

    // score.json 파일에서 플레이어 데이터 리스트 로드
    private List<PlayerData> LoadPlayerData()
    {
        // 파일이 존재하지 않으면 빈 리스트 반환
        if (!File.Exists(filePath))
        {
            return null;
        }

        // 파일 내용을 읽어와 리스트로 변환
        string json = File.ReadAllText(filePath);
        PlayerDataList playerDataList = JsonUtility.FromJson<PlayerDataList>(json);
        return playerDataList.players;
    }

    void Update() {
        // <-키를 누르면 메인 화면으로 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("main");
        }
    }
}
