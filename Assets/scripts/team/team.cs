using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class team : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    public bool isAttackDamage = false;
    public GameObject target = null;
    public float Speed = 5f; // 이동 속도
    public float moveDuration = 0.1f; // 이동 애니메이션 시간
    public float hp = 300f;
    public AudioClip[] audioClips; // 여러 개의 오디오 클립 배열
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    private float detectionRange = 14f; // 적을 감지하는 범위
    private bool isMove = false;
    private bool isMoveMotionSwitch = false;
    public bool isAttackMotion = false;
    public bool isHit = false;
    private bool isDeath = false;
    private bool isAttackFailed = false;
    private bool isGuard = false;
    private ParticleSystem guardEffect; // 가드 이펙트
    private ParticleSystem hitEffect; // 피격 이펙트

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.Log("Rigidbody 컴포넌트가 할당되지 않았습니다!");
        }

        if (animator == null)
        {
            Debug.Log("Animator 컴포넌트가 할당되지 않았습니다!");
        }

        // 애니메이터에서 루트 모션 비활성화
        if (animator != null)
        {
            animator.applyRootMotion = false;
        }

        // Y축 회전을 고정하여 캐릭터가 넘어지지 않게 함
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // 이펙트 모체
        GameObject[] findEffect = GameObject.FindGameObjectsWithTag("guardParticle");
        foreach (GameObject effect in findEffect)
        {
            guardEffect = effect.GetComponent<ParticleSystem>();
        }

        GameObject[] findHitEffect = GameObject.FindGameObjectsWithTag("hitParticle");
        foreach (GameObject effect in findHitEffect)
        {
            hitEffect = effect.GetComponent<ParticleSystem>();
        }

        audioSource1 = GetComponent<AudioSource>();
        if (audioSource1 == null)
        {
            Debug.LogError("AudioSource component missing from this game object. Please add one.");
        } else {
            audioSource1.maxDistance = 60f;
            audioSource1.spatialBlend = 1f;
        }

        audioSource2 = gameObject.AddComponent<AudioSource>();
        if (audioSource2 == null)
        {
            Debug.LogError("AudioSource component missing from this game object. Please add one.");
        } else {
            audioSource2.maxDistance = 60f;
            audioSource2.spatialBlend = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isAttackMotion = animator.GetBool("isAttackMotion");
        isGuard = animator.GetBool("isGuard");
        isDeath = animator.GetBool("isDeath");
        
        if (isDeath || isAttackFailed) {
            return;
        }

        if (hp <= 0) {
            Death();
        }
    }

    void FixedUpdate() {
        if (isDeath || isAttackFailed) {
            return;
        }

        // 주변에 타겟이 있는지 찾기
        if (target == null) {
            FindTarget();
        }
        
        // 타겟이 있으면 타겟으로 이동
        if (target) {
            MoveToTarget();
        }
    }

     void FindTarget() {
        // 감지범위 주변에 적 타겟이 있는지 찾기
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in colliders)
        {
            // 타겟이 없으면 취소
            if (collider.CompareTag("Enemy"))
            {
                target = collider.gameObject;
                break;
            }
        }
    }

    void MoveToTarget()
    {
        if (isAttackMotion || isHit) {
            return;
        }

        Transform targetTransform = target.transform;
        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

        Vector3 targetPosition = targetTransform.position;
        targetPosition.y = transform.position.y; // 현재 위치의 y값으로 설정하여 y값을 무시

        // 포착거리이면서 사정거리밖일 경우 뛰어오기
        if (distanceToTarget < detectionRange && distanceToTarget > 3f)
        {
            Vector3 direction = (targetTransform.position - transform.position).normalized;

            transform.LookAt(targetPosition);
            rb.MovePosition(transform.position + direction * Speed * Time.fixedDeltaTime);
            MoveAni(true);
        }

        // 사정거리 안에 들어왔을 경우
        else if (distanceToTarget <= 3f) {
            transform.LookAt(targetPosition);
            MoveAni(false);
            ActionChoice();
        }

        // 아니면 타겟 해제
        else {
            MoveAni(false);
            target = null;
        }
    }

    void MoveAni(bool setMove) {
        if (setMove) {
            if (isMoveMotionSwitch) {
                return;
            }
            
            animator.SetBool("isMove", true);
            animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration / 4 * Time.fixedDeltaTime));
            animator.CrossFade("Move", moveDuration);

            isMoveMotionSwitch = true;

            isMove = true;
        }

        else {
            if (!isMoveMotionSwitch) {
                return;
            }

            animator.SetBool("isMove", false);
            animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration / 4 * Time.fixedDeltaTime));
            animator.CrossFade("Idle", moveDuration);

            isMoveMotionSwitch = false;

            isMove = false;
        }
    }

    void ActionChoice() {
        if (isAttackMotion || isGuard) {
            return;
        }

        float random = RandomFunction(0, 4);
        switch (random) {
            case 0: {
                Attack1();
                break;
            }

            default: {
                Guard();
                break;
            }
        }
    }

    float RandomFunction(int min, int max)
    {
        return Random.Range(min, max);
    }

    void Guard() {
        animator.SetBool("isGuard", true);
        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
        animator.CrossFade("GuardStop", moveDuration);

        StartCoroutine(ActionWaitCoroutine(1f));
    }

    void Attack1() {
        animator.SetInteger("attack", 1);
        animator.SetBool("isAttackMotion", true);
        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
        animator.CrossFade("Attack1", moveDuration);
        StartCoroutine(AttackMoveForwardCoroutine(0.3f, 0.7f));
        StartCoroutine(AttackedCoroutine(0.3f, 0.6f));
    }

    IEnumerator ActionWaitCoroutine(float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        animator.SetBool("isGuard", false);
    }

    IEnumerator AttackMoveForwardCoroutine(float moveDuration, float delayDuration)
    {
        float elapsedTime = 0f;

        // 0.2초 동안 대기
        yield return new WaitForSeconds(delayDuration);

        // 0.3초 동안 이동
        while (elapsedTime < moveDuration)
        {
            // 이동할 거리 계산
            float distance = Speed * Time.fixedDeltaTime;

            // 앞으로 이동
            transform.Translate((Vector3.forward * distance) / 2);

            // 경과 시간 업데이트
            elapsedTime += Time.fixedDeltaTime;

            // 다음 프레임까지 대기
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator AttackedCoroutine(float attackDuration, float delayDuration)
    {   
        isAttackDamage = false;

        // N초 동안 대기
        yield return new WaitForSeconds(delayDuration);

        // 맞는중엔 공격안함
        if (!isHit) {
            isAttackDamage = true;
        }

        // N초 동안 공격
        yield return new WaitForSeconds(attackDuration);
        isAttackDamage = false;
    }

    public void Death() {
        animator.SetBool("isDeath", true);
        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
        animator.CrossFade("Death", 1f);

        // 1분 뒤 제거
        StartCoroutine(DestroyCoroutine(60f));
    }

    public void Hit(float damage) {
        if (isHit) {
            return;
        }

        if (hp <= 0) {
            return;
        }
        
        StartCoroutine(HitWait(0.4f));
        
        // 막음
        if (isGuard) {
            // 가드음
            audioSource2.pitch = 0.7f;
            audioSource2.clip = audioClips[0];
            audioSource2.loop = false;
            audioSource2.Play();

            // 가드 이펙트
            ParticleSystem instantiatedEffect = Instantiate(guardEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            instantiatedEffect.Play();
            Destroy(instantiatedEffect.gameObject, 1f); // 적절한 시간 후에 파티클 시스템 삭제
            
            return;
        }

        // 피격음
        bool random = RandomFunction(0, 2) == 1 ? true : false;
        if (random) {
            audioSource1.pitch = 0.7f;
            audioSource1.clip = audioClips[1];
            audioSource1.loop = false;
            audioSource1.Play();
        }

        else {
            audioSource1.pitch = 0.8f;
            audioSource1.clip = audioClips[2];
            
            audioSource1.Play();
        }

        // 피격 이펙트
        ParticleSystem instantiatedHitEffect = Instantiate(hitEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        instantiatedHitEffect.Play();
        Destroy(instantiatedHitEffect.gameObject, 1f); // 적절한 시간 후에 파티클 시스템 삭제
        
        animator.SetBool("isHit", true);
        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
        animator.CrossFade("Hit", 0.01f);

        hp -= damage;
    }

    IEnumerator HitWait(float waitDuration)
    {
        isHit = true;

        yield return new WaitForSeconds(waitDuration);
        isHit = false;
    }

    IEnumerator AttackFailedWait(float waitDuration)
    {
        isAttackFailed = true;

        yield return new WaitForSeconds(waitDuration);
        isAttackFailed = false;
    }

    // 30초뒤 제거
    IEnumerator DestroyCoroutine(float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        Destroy(gameObject);
    }
}