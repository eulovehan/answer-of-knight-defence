using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float Speed = 5f; // 이동 속도
    public float attackPower = 1f; // 공격력 배율
    public bool isGuard = false;
    public float totalHp = 200f; // 전체 체력
    public float hp = 200f; // 체력
    public AudioClip[] audioClips; // 여러 개의 오디오 클립 배열
    public bool isStop = true; // 정지 상태
    public int coin = 500; // 코인
    public int shot = 0; // 총알
    public int totalShot = 300; // 전체 총알
    public int weapon = 1; // 무기
    public GameObject[] weapon1; // 무기1
    public GameObject[] weapon2; // 무기2
    public GameObject[] weapon2UI; // 무기2 UI
    public GameObject focus; // 조준점
    public bool isFocus = false; // 조준 상태
    public float GunDamege = 9f; // 총 데미지
    public LayerMask ignoreLayer; // Inspector에서 설정 가능
    public int score = 0; // 점수
    public bool isDeath = false; // 사망 상태
    public int GunMotion = 0; // 총 발사 모션
    public UnityEngine.UI.Image GameOverFade; // 게임 오버 페이드

    private Rigidbody rb;
    private Animator animator;
    private bool isMove = false;
    private bool isMoveMotionSwitch = false;
    private bool isGuardMoveMotionSwtich = false;
    public bool isAttack = false;
    public bool isAttackDamage = false;
    private bool isGuardStopMotionSwitch = false;
    private int attackAction = 0;
    private float moveDuration = 0.1f;
    private bool isBackMove = false;
    private bool isHit = false;
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioGunSource3;
    private ParticleSystem guardEffect; // 가드 이펙트
    private ParticleSystem hitEffect; // 히트 이펙트
    private GameObject textUI; // 텍스트 UI
    private bool isReload = false; // 재장전 상태
    private float shotTime = 0f; // 총알 발사 간격
    
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

        audioSource1 = GetComponent<AudioSource>();
        if (audioSource1 == null)
        {
            Debug.LogError("AudioSource component missing from this game object. Please add one.");
        }

        audioSource2 = gameObject.AddComponent<AudioSource>();
        if (audioSource2 == null)
        {
            Debug.LogError("AudioSource component missing from this game object. Please add one.");
        }

        audioGunSource3 = gameObject.AddComponent<AudioSource>();
        if (audioGunSource3 == null)
        {
            Debug.LogError("AudioSource component missing from this game object. Please add one.");
        }

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

        // 텍스트 UI
        GameObject[] findUI = GameObject.FindGameObjectsWithTag("textUI");
        foreach (GameObject ui in findUI)
        {
            textUI = ui;
        }

        // gameover fade find
        GameObject[] findFade = GameObject.FindGameObjectsWithTag("GameOverFade");
        foreach (GameObject fade in findFade)
        {
            GameOverFade = fade.GetComponent<UnityEngine.UI.Image>();
        }
        
        StartCoroutine(Openning());
    }

    void Update() {
        // 사망했으면 모든 액션 비활성화
        if (isDeath) {
            return;
        }
        
        // 마우스 오른쪽 버튼이 클릭되면 가드
        if (Input.GetMouseButton(1) && weapon == 1) {
            isGuard = true;
        }

        // 마우스 오른쪽 버튼이 떼지면 가드 해제
        else {
            isGuard = false;
        }

        animator.SetBool("isGuard", isGuard);
        animator.SetInteger("weapon", weapon);
        
        // 스탑 가드 상태이고 락온이 된 상태일시 적을 바라보기
        if (isGuard && !isMove) {
            bool isLockOn = Camera.main.GetComponent<mainCamera>().isLockOn;
            if (!isLockOn) {
                return;
            }
            
            GameObject currentTarget = Camera.main.GetComponent<mainCamera>().currentTarget;
            
            Vector3 targetPosition = currentTarget.transform.position;

            // rotate y 30정도 오차
            Vector3 direction = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Quaternion additionalRotation = Quaternion.Euler(0, 50, 0); // y값에 50도 회전을 추가
            transform.rotation = rotation * additionalRotation;
        }

        // 1번 키를 눌렀을 때 weapon 변수를 1로 설정
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = 1;
            
            // weapon1를 모두 활성화, weapon2를 모두 비활성화
            foreach (GameObject weapon in weapon1)
            {
                weapon.SetActive(true);
            }

            foreach (GameObject weapon in weapon2)
            {
                weapon.SetActive(false);
            }

            // weapon1 ui를 모두 활성화, weapon2 ui를 모두 비활성화
            foreach (GameObject weaponUI in weapon2UI)
            {
                weaponUI.SetActive(false);
            }
        }

        // 2번 키를 눌렀을 때 weapon 변수를 2로 설정
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = 2;
            
            // weapon1를 모두 비활성화, weapon2를 모두 활성화
            foreach (GameObject weapon in weapon1)
            {
                weapon.SetActive(false);
            }

            foreach (GameObject weapon in weapon2)
            {
                weapon.SetActive(true);
            }

            // weapon1 ui를 모두 비활성화, weapon2 ui를 모두 활성화
            foreach (GameObject weaponUI in weapon2UI)
            {
                weaponUI.SetActive(true);
            }
        }
    }

    void FixedUpdate() {
        // 사망했으면 모든 액션 비활성화
        if (isDeath) {
            return;
        }
        
        Move();
        GuardStop();

        // 체력이 0이하면 사망
        if (hp <= 0) {
            Death();
        }
    }

    void LateUpdate()
    {
        // 사망했으면 모든 액션 비활성화
        if (isDeath) {
            return;
        }
        
        switch (weapon) {
            case 1: {
                // 마우스 왼쪽 버튼이 클릭되면 공격
                if (Input.GetMouseButtonDown(0)) {
                    Weapon1Attack();
                }
                
                break;
            }

            case 2: {
                // 마우스 왼쪽 버튼이 클릭되면 공격
                if (Input.GetMouseButton(0) && shot > 0 && !isReload) {
                    Weapon2Attack();

                    // 플레이어가 보는 방향을 카메라 보는 방향에서 10도만큼 오른쪽으로 회전
                    transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y + 30, 0);
                }

                else {
                    animator.SetBool("isMoveGunShot", false);
                    animator.SetBool("isStopGunShot", false);
                    
                    // 모션 전환
                    if (GunMotion != 0 && isMove) {
                        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                        animator.CrossFade("GunMove", moveDuration / 4);
                    }

                    else if (GunMotion != 0 && !isMove) {
                        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                        animator.CrossFade("GunIdle", moveDuration / 4);
                    }
                    
                    GunMotion = 0;
                }
                
                // 오른쪽 버튼 누르면 조준
                if (Input.GetMouseButton(1) && !isReload) {
                    focus.SetActive(true);
                    isFocus = true;
                }

                else {
                    focus.SetActive(false);
                    isFocus = false;
                }

                // r키 누르면 재장전
                if (Input.GetKeyDown(KeyCode.R)) {
                    Reload();
                }

                break;
            }
        }

        if (GunMotion != 0) {
            if (!audioGunSource3.isPlaying) {
                audioGunSource3.loop = true;
                audioGunSource3.pitch = 0.9f;
                audioGunSource3.volume = 0.6f;
                audioGunSource3.clip = audioClips[4];
                audioGunSource3.Play();
            }
        }
        
        else {
            audioGunSource3.loop = false;
            audioGunSource3.Stop();
        }
    }

    void Move()
    {
        // 공격 / 히트 중 액션불가
        if (isAttack || isHit || isReload) {
            return;
        }
        
        float moveX = Input.GetAxis("Horizontal"); // A, D 키 또는 좌우 화살표 입력
        float moveZ = Input.GetAxis("Vertical");   // W, S 키 또는 상하 화살표 입력

        // 이동안할 시 애니메이션 변경
        if (moveX == 0 && moveZ == 0) {
            if (weapon == 1) {
                Weapon1MoveAni(false);
            }

            else if (weapon == 2) {
                Weapon2MoveAni(false);
            }
            
            isBackMove = false;

            return;
        }

        float MoveSpeed = isGuard ? Speed / 3.5f : Speed;

        // 총쏘는중이 아니면
        if (GunMotion == 0) {
            // 앞으로 이동
            if (moveZ > 0) {
                Quaternion playerRotation = Camera.main.transform.rotation;
                Quaternion targetRotation = Quaternion.Euler(0f, playerRotation.eulerAngles.y, 0f);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 15f);
            }
            
            // 뒤쪽 키를 누르면 (반복작업되면 안됨)
            if (moveZ < 0 && !isBackMove) {
                Quaternion playerRotation = Camera.main.transform.rotation;

                // 현재 플레이어의 Y축 회전값에 180도를 더한 후 회전
                transform.rotation = Quaternion.Euler(0f, playerRotation.eulerAngles.y + 180f, 0f);
                isBackMove = true;
            }

            else if (!(moveZ < 0)) {
                isBackMove = false;
            }

            // 왼쪽 키를 누르면
            if (moveX < 0)
            {
                // 현재 메인 카메라의 왼쪽 방향 벡터를 가져옴
                Vector3 cameraLeftDirection = -Camera.main.transform.right;

                // 카메라의 왼쪽 방향 벡터를 기준으로 플레이어를 회전
                Quaternion targetRotation = Quaternion.LookRotation(cameraLeftDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 15f);
            }
            
            // 오른쪽 키를 누르면
            if (moveX > 0)
            {
                // 현재 메인 카메라의 오른쪽 방향 벡터를 가져옴
                Vector3 cameraRightDirection = Camera.main.transform.right;

                // 카메라의 오른쪽 방향 벡터를 기준으로 플레이어를 회전
                Quaternion targetRotation = Quaternion.LookRotation(cameraRightDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
            }

            if (!isStop) {
                rb.MovePosition(transform.position + transform.forward * MoveSpeed * Time.fixedDeltaTime);
            }
        }
        
        // 총 쏘는중이면 현재 위치에서 상하좌우 이동
        else {
            if (moveZ > 0) {
                rb.MovePosition(transform.position + transform.forward * MoveSpeed * Time.fixedDeltaTime);
            }

            if (moveZ < 0) {
                rb.MovePosition(transform.position - transform.forward * MoveSpeed * Time.fixedDeltaTime);
            }

            if (moveX < 0) {
                rb.MovePosition(transform.position - transform.right * MoveSpeed * Time.fixedDeltaTime);
            }

            if (moveX > 0) {
                rb.MovePosition(transform.position + transform.right * MoveSpeed * Time.fixedDeltaTime);
            }
        }
        
        if (weapon == 1) {
            Weapon1MoveAni(true);
        }

        else if (weapon == 2) {
            Weapon2MoveAni(true);
        }
    }

    void Weapon1MoveAni(bool setMove) {
        if (setMove && !isStop) {
            float soundPitch = 1f;
            
            if (!isGuard) {
                if (isMoveMotionSwitch) {
                    return;
                }

                isMove = true;
                animator.SetBool("isMove", true);
                animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                animator.CrossFade("Move", moveDuration / 2);

                soundPitch = 0.9f;

                isGuardMoveMotionSwtich = false;
                isMoveMotionSwitch = true;
            }

            else {
                if (isGuardMoveMotionSwtich) {
                    return;
                }

                isMove = true;
                animator.SetBool("isMove", true);
                animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                animator.CrossFade("GuardMove", moveDuration / 2);

                // 오디오 재생 속도 조절
                soundPitch = soundPitch / 2.2f;
                
                isGuardMoveMotionSwtich = true;
                isMoveMotionSwitch = false;
            }

            // 오디오 재생
            audioSource1.loop = true;
            audioSource1.pitch = soundPitch;
            audioSource1.clip = audioClips[0];
            audioSource1.Play();
        }

        else {
            if (!isGuard) {
                if (!isMoveMotionSwitch) {
                    return;
                }

                isMove = false;
                animator.SetBool("isMove", false);
                animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                animator.CrossFade("Idle", moveDuration);

                isGuardMoveMotionSwtich = false;
                isMoveMotionSwitch = false;
            }

            else {
                if (!isGuardMoveMotionSwtich) {
                    return;
                }

                isMove = false;
                if (!Input.GetMouseButton(1)) {
                    animator.SetBool("isMove", false);
                    animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                    animator.CrossFade("Idle", moveDuration);
                }

                else {
                    animator.SetBool("isMove", false);
                    animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                    animator.CrossFade("GuardStop", moveDuration / 2);

                    isGuardStopMotionSwitch = true;
                }
                
                isGuardMoveMotionSwtich = false;
                isMoveMotionSwitch = false;
            }

            // 이동 중이 아니면 오디오 중지
            if (audioSource1.isPlaying) {
                audioSource1.loop = false;
                audioSource1.Stop();
            }
        }
    }

    void Weapon2MoveAni(bool setMove) {
        if (setMove && !isStop) {
            float soundPitch = 0.9f;
            
            if (isMoveMotionSwitch) {
                return;
            }

            isMove = true;
            animator.SetBool("isMove", true);
            animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
            animator.CrossFade("GunMove", moveDuration / 4);

            isGuardMoveMotionSwtich = false;
            isMoveMotionSwitch = true;

            // 오디오 재생
            audioSource1.loop = true;
            audioSource1.pitch = soundPitch;
            audioSource1.clip = audioClips[0];
            audioSource1.Play();
        }

        else {
            if (!isMoveMotionSwitch) {
                return;
            }

            isMove = false;
            animator.SetBool("isMove", false);
            animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
            animator.CrossFade("GunIdle", moveDuration);

            isGuardMoveMotionSwtich = false;
            isMoveMotionSwitch = false;

            // 이동 중이 아니면 오디오 중지
            if (audioSource1.isPlaying) {
                audioSource1.loop = false;
                audioSource1.Stop();
            }
        }
    }
    
    void GuardStop() {
        if (isMove || isAttack || isHit || weapon != 1) {
            return;
        }
        
        if (isGuard) {
            if (isGuardStopMotionSwitch) {
                return;
            }

            animator.SetBool("isMove", false);
            animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
            animator.CrossFade("GuardStop", moveDuration / 2);

            isGuardStopMotionSwitch = true;
        }

        else {
            if (!isGuardStopMotionSwitch) {
                return;
            }
            
            animator.SetBool("isMove", false);
            animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
            animator.CrossFade("Idle", moveDuration);

            isGuardStopMotionSwitch = false;
        }

        isBackMove = false;
    }

    void Weapon1Attack() {
        if (isAttack || isHit || isStop) {
            return;
        }
        
        // 기존 소리 제거
        if (audioSource1.isPlaying) {
            audioSource1.loop = false;
            audioSource1.Stop();
        }

        if (audioSource2.isPlaying) {
            audioSource2.loop = false;
            audioSource2.Stop();
        }
        
        float attackDuration = 0.05f;
        
        isMove = false;
        animator.SetBool("isMove", false);
        animator.SetInteger("attackAction", attackAction);
        animator.SetBool("isAttack", true);
        
        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, attackDuration * Time.fixedDeltaTime));

        if (attackAction == 0) {
            animator.CrossFade("Attack1", attackDuration);
            attackAction = 1;
            StartCoroutine(AttackMoveForwardCoroutine(0.4f, 0.1f));
            StartCoroutine(AttackedCoroutine(0.7f, 0.05f, 1));
            StartCoroutine(AttackedSoundCoroutine(0.1f, 0.6f));
        }

        else if (attackAction == 1) {
            animator.CrossFade("Attack2", attackDuration);
            attackAction = 2;
            StartCoroutine(AttackMoveForwardCoroutine(0.6f, 0.3f, 3f));
            StartCoroutine(AttackedCoroutine(0.4f, 0.1f, 1, false));
            StartCoroutine(AttackedSoundCoroutine(0.1f, 0.58f));
            StartCoroutine(AttackedCoroutine(0.4f, 0.65f, 1));
            StartCoroutine(AttackedSoundCoroutine(0.65f, 0.55f));
        }
        
        else {
            animator.CrossFade("Attack3", attackDuration);
            attackAction = 0;
            StartCoroutine(AttackMoveForwardCoroutine(0.6f, 0.55f));
            StartCoroutine(AttackRotateForwardCoroutine(0.6f, 0.4f, 1.5f, 600f));
            StartCoroutine(AttackedCoroutine(0.5f, 0.55f, 1.5f));
            StartCoroutine(AttackedSoundCoroutine(0.55f, 0.52f));
        }

        // 기존 모션 스위치 모두 취소
        isMoveMotionSwitch = false;
        isGuardMoveMotionSwtich = false;
    }

    void Weapon2Attack() {
        if (isAttack || isHit || isStop) {
            return;
        }
        
        // 기존 소리 제거
        if (audioSource1.isPlaying) {
            audioSource1.loop = false;
            audioSource1.Stop();
        }

        if (audioSource2.isPlaying) {
            audioSource2.loop = false;
            audioSource2.Stop();
        }

        // 움직일때 공격
        if (isMove) {
            animator.SetBool("isMoveGunShot", true);
            animator.SetBool("isStopGunShot", false);

            if (GunMotion != 1) {
                animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                animator.CrossFade("GunMoveAttack", moveDuration / 4);
            }

            GunMotion = 1;
        }

        else {
            animator.SetBool("isMoveGunShot", false);
            animator.SetBool("isStopGunShot", true);

            if (GunMotion != 2) {
                animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
                animator.CrossFade("GunAttack", moveDuration / 4);
            }

            GunMotion = 2;
        }

        // 0.2초마다 공격
        shotTime += Time.deltaTime;
        if (shotTime > 0.2f) {
            // 총구가 바라보는 방향으로 총알 발사하고 Enemy 태그를 가진 적을 공격
            // 총알이 맞는 범위는 좌우 상하 0.2
            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.4f, 0.4f, 0));
            RaycastHit hit;

            float sphereRadius = 0.2f;

            if (Physics.SphereCast(rayOrigin, sphereRadius, Camera.main.transform.forward, out hit, 100f, ~ignoreLayer)) {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<knight>().Hit(GunDamege, true);
                }
            }

            shot --;
            shotTime = 0f;
        }
    }

    IEnumerator AttackMoveForwardCoroutine(float moveDuration, float delayDuration, float slow = 2f)
    {
        float elapsedTime = 0f;

        // N초 동안 대기
        yield return new WaitForSeconds(delayDuration);

        // N초 동안 이동
        while (elapsedTime < moveDuration)
        {
            // 이동할 거리 계산
            float distance = Speed * Time.fixedDeltaTime;

            // 앞으로 이동
            transform.Translate((Vector3.forward * distance) / slow);

            // 경과 시간 업데이트
            elapsedTime += Time.fixedDeltaTime;

            // 다음 프레임까지 대기
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator AttackRotateForwardCoroutine(float moveDuration, float delayDuration, float slow = 2f, float rotate = 0f)
    {
        float elapsedTime = 0f;

        // N초 동안 대기
        yield return new WaitForSeconds(delayDuration);

        // N초 동안 이동
        while (elapsedTime < moveDuration)
        {
            // 이동할 거리 계산
            float distance = Speed * Time.fixedDeltaTime;

            // 회전 주기
            if (rotate != 0f) {
                transform.Rotate(Vector3.up * rotate * Time.fixedDeltaTime);
            }

            // 경과 시간 업데이트
            elapsedTime += Time.fixedDeltaTime;

            // 다음 프레임까지 대기
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator AttackedCoroutine(float attackDuration, float delayDuration, float power, bool isAttackReturn = true)
    {   
        isAttackDamage = false;
        isAttack = true;
        attackPower = power;

        // N초 동안 대기
        yield return new WaitForSeconds(delayDuration);
        
        // 맞는중엔 공격안함
        if (!isHit) {
            isAttackDamage = true;
        }

        // N초 동안 공격
        yield return new WaitForSeconds(attackDuration);
        isAttackDamage = false;
        
        // 리턴받을지에 대한여부 (연속 공격 등 제어)
        if (isAttackReturn) {
            isAttack = false;
        }

        attackPower = 1f;
    }

    IEnumerator AttackedSoundCoroutine(float delayDuration, float audioPitch = 0.5f)
    {  
        // N초 동안 대기
        yield return new WaitForSeconds(delayDuration);
        
        // 맞는중엔 공격안함
        if (!isHit) {
            // 검음 출력
            if (!audioSource1.isPlaying) {
                audioSource1.loop = false;
                audioSource1.pitch = audioPitch;
                audioSource1.clip = audioClips[1];
                audioSource1.Play();
            }

            else {
                audioSource2.loop = false;
                audioSource2.pitch = audioPitch;
                audioSource2.clip = audioClips[1];
                audioSource2.Play();
            }
        }
    }

    public AttackResult Hit(float damege) {
        // 사망했으면 모든 액션 비활성화
        if (isDeath) {
            return AttackResult.Wait;
        }
        
        if (isHit) {
            return AttackResult.Wait;
        }
        
        if (isGuard) {
            StartCoroutine(HitWait(0.4f));

            // 가드음
            audioSource1.loop = false;
            audioSource2.pitch = 0.8f;
            audioSource2.clip = audioClips[2];
            audioSource2.Play();

            // 가드 이펙트
            ParticleSystem instantiatedEffect = Instantiate(guardEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            instantiatedEffect.Play();
            Destroy(instantiatedEffect.gameObject, 1f); // 적절한 시간 후에 파티클 시스템 삭제

            return AttackResult.Guard;
        }

        // 효과음
        audioSource1.loop = false;
        audioSource1.pitch = 1f;
        audioSource1.clip = audioClips[3];
        audioSource1.Play();

        animator.SetBool("isHit", true);
        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
        animator.CrossFade("Hit", 0);

        // 히트 이펙트
        ParticleSystem instantiatedHitEffect = Instantiate(hitEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        instantiatedHitEffect.Play();
        Destroy(instantiatedHitEffect.gameObject, 1f); // 적절한 시간 후에 파티클 시스템 삭제

        hp -= damege;

        StartCoroutine(HitWait(0.4f));
        return AttackResult.Success;
    }

    IEnumerator HitWait(float waitDuration)
    {
        isHit = true;

        yield return new WaitForSeconds(waitDuration);
        isHit = false;
    }

    IEnumerator Openning()
    {
        yield return new WaitForSeconds(1f);
        isStop = false;
    }

    public void Kill() {
        int random = Random.Range(1, 5);
        coin += random + 10;
        score += 50;
    }

    void Reload() {
        if (isReload) {
            return;
        }

        if (totalShot == 0) {
            return;
        }

        // 효과음
        audioSource1.loop = false;
        audioSource1.pitch = 1f;
        audioSource1.volume = 1f;
        audioSource1.clip = audioClips[5];
        audioSource1.Play();

        isReload = true;
        animator.SetBool("reload", true);
        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
        animator.CrossFade("Reload", moveDuration);
    }

    public void ReloadEnd() {
        // 효과음
        audioSource2.Stop();
        audioSource2.loop = false;
        audioSource2.pitch = 1f;
        audioSource2.volume = 1f;
        audioSource2.clip = audioClips[6];
        audioSource2.Play();
        
        isReload = false;
        
        // 최대 장전 개수 30, 30개 미만이면 남은 개수만큼
        if (totalShot < 30) {
            shot = totalShot;
            totalShot = 0;
        }

        else {
            int setShot = 30 - shot;
            
            shot = 30;
            totalShot -= setShot;
        }

        animator.SetBool("reload", false);
    }

    public void Death(bool isEndPoin = false) {
        isDeath = true;
        GunMotion = 0;
        animator.SetBool("isDeath", true);
        animator.SetLayerWeight(0, Mathf.Lerp(animator.GetLayerWeight(0), 0, moveDuration * Time.fixedDeltaTime));
        animator.CrossFade("Death", moveDuration);

        if (!isEndPoin) {
            StartCoroutine(DeathEndCoroutine(3f));
        }

        else {
            StartCoroutine(EndPointEndCoroutine(3f));
        }
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float fadeDuration = 3f;
        float startAlpha = GameOverFade.color.a;
        float elapsedTime = 0f;

        // 페이드 아웃 동안 이미지를 점점 어둡게 만듭니다.
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
            GameOverFade.color = new Color(GameOverFade.color.r, GameOverFade.color.g, GameOverFade.color.b, newAlpha);
            yield return null;
        }

        // 페이드 아웃 완료 후 씬을 전환합니다.
        SceneManager.LoadScene("gameover");
    }

    IEnumerator DeathEndCoroutine(float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        textUI.GetComponent<text>().TextView("YOU DIED", 3f);

        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOut("gameover"));
    }

    IEnumerator EndPointEndCoroutine(float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        textUI.GetComponent<text>().TextView("Defense failed", 3f);

        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOut("gameover"));
    }
}