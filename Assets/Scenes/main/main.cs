using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ->키를 누르면 스코어 화면으로 이동
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("score");
        }
    }
}
