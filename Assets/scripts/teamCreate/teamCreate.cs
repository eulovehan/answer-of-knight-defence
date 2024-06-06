using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamCreate : MonoBehaviour
{
    public GameObject team;
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;
    public GameObject point4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTeam() {
        // 랜덤 4개 중 한곳에 캐릭터 생성
        int random = Random.Range(1, 5);
        switch (random) {
            case 1:
                Instantiate(team, point1.transform.position, point1.transform.rotation);
                break;
            case 2:
                Instantiate(team, point2.transform.position, point2.transform.rotation);
                break;
            case 3:
                Instantiate(team, point3.transform.position, point3.transform.rotation);
                break;
            case 4:
                Instantiate(team, point4.transform.position, point4.transform.rotation);
                break;
        }
    }

    float RandomFunction(int min, int max)
    {
        return Random.Range(min, max);
    }
}
