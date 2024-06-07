using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introCamera : MonoBehaviour
{
    public UnityEngine.UI.Image Fade;
    private bool isFadeOut = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        StartCoroutine(Story());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        // 알파값을 1로 초기화
        Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.b, 1);

        float fadeDuration = 10f;
        float targetAlpha = 0.3f;
        float startAlpha = Fade.color.a;
        float elapsedTime = 0f;

        while (Fade.color.a > targetAlpha)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.b, newAlpha);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float fadeDuration = 6f;
        float targetAlpha = 1f;
        float startAlpha = Fade.color.a;
        float elapsedTime = 0f;

        while (Fade.color.a < targetAlpha)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            Fade.color = new Color(Fade.color.r, Fade.color.g, Fade.color.b, newAlpha);
            yield return null;
        }

        // 3초 대기 후 game씬으로 이동
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("game");
        
        yield return null;
    }

    IEnumerator Story() {
        // 초기 시작 위치
        transform.position = new Vector3(-157.19f, 27.17f, 77.54f);
        transform.rotation = Quaternion.Euler(51.45f, -156.46f, 0f);

        Vector3 targettransform = new Vector3(-151.54f, 20.36f, 56.36f);
        
        // 10초 대기
        yield return StartCoroutine(CameraMove(transform, targettransform, 10f));

        // 카메라 이동
        transform.position = new Vector3(-155.8f, 1.34f, 50.6f);
        transform.rotation = Quaternion.Euler(1.051f, -99.357f, 12.372f);

        Vector3 target2transform = new Vector3(-155.9f, 1.47f, 51.18f);
        yield return StartCoroutine(CameraMove(transform, target2transform, 11f, true));
        
        yield return null;
    }

    // 카메라를 천천히 이동시키는 함수
    IEnumerator CameraMove(Transform startTransform, Vector3 endTransform, float duration, bool fadeOut = false) {
        float elapsedTime = 0f;
        Vector3 startPosition = startTransform.position; // 시작 위치 저장
        while (elapsedTime < duration) {
            // 보간하여 카메라 이동
            float t = elapsedTime / duration;
            startTransform.position = Vector3.Lerp(startPosition, endTransform, t);
            elapsedTime += Time.deltaTime;

            if (fadeOut && elapsedTime >= 5f && isFadeOut == false) {
                isFadeOut = true;
                StartCoroutine(FadeOut());
            }

            yield return null;
        }

        startTransform.position = endTransform; // 목표 위치에 도달하면 정확한 위치로 보정

        yield return null;
    }
}
