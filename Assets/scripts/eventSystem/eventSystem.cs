using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class eventSystem : MonoBehaviour
{
    public int Stage = 1;
    public GameObject objectToSpawn; // 생성할 오브젝트
    public AudioClip[] audioClips; // 여러 개의 오디오 클립 배열
    
    private GameObject textUI; // 텍스트 UI
    private AudioSource audioSource1;
    private AudioSource audioSource2;

       
    // Start is called before the first frame update
    void Start()
    {
        // text ui set
        GameObject[] findUI = GameObject.FindGameObjectsWithTag("textUI");
        foreach (GameObject ui in findUI)
        {
            textUI = ui;
        }

        audioSource1 = GetComponent<AudioSource>();
        if (audioSource1 == null)
        {
            Debug.LogError("AudioSource component missing from this game object. Please add one.");
        } else {
            audioSource1.maxDistance = 400f;
            audioSource1.spatialBlend = 1f;
            audioSource1.rolloffMode = AudioRolloffMode.Linear;
        }

        audioSource2 = GetComponent<AudioSource>();
        if (audioSource2 == null)
        {
            Debug.LogError("AudioSource component missing from this game object. Please add one.");
        } else {
            audioSource2.maxDistance = 400f;
            audioSource2.spatialBlend = 1f;
            audioSource2.rolloffMode = AudioRolloffMode.Linear;
        }

        StartCoroutine(Openning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Create(float count) {
        for (int i = 0; i < count; i++) {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
    }

    void StartSound() {
        audioSource1.loop = false;
        audioSource1.pitch = 0.12f;
        audioSource1.clip = audioClips[0];
        audioSource1.Play();
    }

    void CrySound() {
        audioSource2.loop = false;
        audioSource2.pitch = 0.8f;
        audioSource2.clip = audioClips[1];
        audioSource2.Play();
    }

    IEnumerator Openning()
    {
        yield return new WaitForSeconds(10f);
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);

        yield return new WaitForSeconds(10f);
        textUI.GetComponent<text>().TextView("You are defeated if you die or an enemy invades.", 7f);

        yield return new WaitForSeconds(10f);
        
        StartCoroutine(Stage1());
    }

    IEnumerator Stage1()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 1", 8f);
        Create(20);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(15);
        yield return new WaitForSeconds(20f);
        
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);
        yield return new WaitForSeconds(30f);

        StartCoroutine(Stage2());
    }

    IEnumerator Stage2()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 2", 8f);
        Create(20);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(15);
        yield return new WaitForSeconds(50f);
        
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);
        yield return new WaitForSeconds(30f);

        StartCoroutine(Stage3());
    }

    IEnumerator Stage3()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 3", 8f);
        Create(30);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(30);
        yield return new WaitForSeconds(50f);
        
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);
        yield return new WaitForSeconds(30f);

        StartCoroutine(Stage4());
    }

    IEnumerator Stage4()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 4", 8f);
        Create(40);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(50);
        yield return new WaitForSeconds(50f);
        
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);
        yield return new WaitForSeconds(30f);

        StartCoroutine(Stage5());
    }

    IEnumerator Stage5()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 5", 8f);
        Create(60);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(60);
        yield return new WaitForSeconds(50f);
        
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);
        yield return new WaitForSeconds(30f);

        StartCoroutine(Stage6());
    }

     IEnumerator Stage6()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 6", 8f);
        Create(60);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(100);
        yield return new WaitForSeconds(50f);
        
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);
        yield return new WaitForSeconds(30f);

        StartCoroutine(Stage7());
    }

     IEnumerator Stage7()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 7", 8f);
        Create(100);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(60);
        yield return new WaitForSeconds(50f);
        
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);
        yield return new WaitForSeconds(30f);

        StartCoroutine(Stage8());
    }

    IEnumerator Stage8()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 8", 8f);
        Create(120);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(120);
        yield return new WaitForSeconds(50f);
        
        textUI.GetComponent<text>().TextView("잠시 뒤에 적병이 몰려옵니다.", 7f);
        yield return new WaitForSeconds(30f);

        StartCoroutine(Stage9());
    }

    IEnumerator Stage9()
    {
        CrySound();
        StartSound();
        textUI.GetComponent<text>().TextView("Stage 8", 8f);
        Create(100);
        yield return new WaitForSeconds(50f);

        CrySound();
        textUI.GetComponent<text>().TextView("추가 병력이 몰려옵니다!", 8f);
        Create(300);
        yield return new WaitForSeconds(50f);
        
        yield return new WaitForSeconds(30f);

        // StartCoroutine(Stage9());
    }
}