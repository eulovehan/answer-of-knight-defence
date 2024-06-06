using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameOverScore : MonoBehaviour
{
    public TextMeshProUGUI scoreContentText;  // UI 텍스트 요소
    public int score = 1;  // 플레이어 점수
    public string playerName = "noname1";  // 플레이어 이름
    public TMP_InputField inputField;

    private string filePath = "./score.json";  // 파일 경로
    private bool isSave = false;  // 데이터 저장 여부

    // Start is called before the first frame update
    void Start()
    {
        // 마우스 커서 복구
        Cursor.lockState = CursorLockMode.None;
    }

    void Update() {
        // score store 찾기
        GameObject[] findScoreStore = GameObject.FindGameObjectsWithTag("ScoreStore");
        foreach (GameObject find in findScoreStore)
        {
            score = find.GetComponent<scoreSendObject>().score;
            scoreContentText.text = "score: " + score;  // UI 텍스트 업데이트
        }
        
        // Input 클래스를 사용하여 Enter 키를 감지
        if (Input.GetKeyDown(KeyCode.Return))
        {
            playerName = inputField.text;

            if (isSave == false)
            {
                isSave = true;
                SaveData();
            }
        }
    }

    // 데이터 저장
    void SaveData()
    {
        // 현재 플레이어 데이터 생성
        PlayerData currentPlayerData = new PlayerData();
        currentPlayerData.name = playerName;
        currentPlayerData.score = score;
        currentPlayerData.date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        // score.json 파일에 현재 플레이어 데이터 저장
        SavePlayerData(currentPlayerData);
    }

    // 현재 플레이어 데이터를 score.json 파일에 저장
    private void SavePlayerData(PlayerData playerData)
    {
        // score.json 파일에 이미 데이터가 있는지 확인
        List<PlayerData> allPlayerData = LoadPlayerData();
        if (allPlayerData == null)
        {
            // 파일이 존재하지 않으면 새로운 리스트 생성
            allPlayerData = new List<PlayerData>();
        }

        // 리스트에 현재 플레이어 데이터 추가
        allPlayerData.Add(playerData);

        // 리스트를 JSON 형식으로 변환하여 파일에 저장
        string json = JsonUtility.ToJson(new PlayerDataList { players = allPlayerData });
        File.WriteAllText(filePath, json);
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
}