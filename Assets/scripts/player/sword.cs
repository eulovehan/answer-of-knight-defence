using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    public GameObject parent;
    public float weaponDamage = 15f;
    private List<GameObject> collidedObjects = new List<GameObject>();

    void OnTriggerEnter(Collider collision)
    {
        if (parent.CompareTag("Player")) {
            WithPlayer(collision);
        }

        else if (parent.CompareTag("Team")) {
            WithTeam(collision);
        }
    }

    void WithPlayer(Collider collision) {
        if (parent.GetComponent<player>().isAttackDamage == false) {
            return;
        }

        GameObject collidedObject = collision.gameObject;

        // 충돌 쿨타임에 있는 경우 허용하지 않음
        if (collidedObjects.Contains(collidedObject)) {
            return;
        }

        // 대미지 주기
        if (collidedObject.CompareTag("Enemy")) {
            // 플레이어의 파워 배율 적용
            float damage = parent.GetComponent<player>().attackPower * weaponDamage;
            
            collidedObject.GetComponent<knight>().Hit(damage);
            collidedObjects.Add(collidedObject);
            StartCoroutine(AttackWaitCoroutine(1f, collidedObject));
        }
    }

    void WithTeam(Collider collision) {
        if (parent.GetComponent<team>().isAttackDamage == false) {
            return;
        }

        GameObject collidedObject = collision.gameObject;

        // 충돌 쿨타임에 있는 경우 허용하지 않음
        if (collidedObjects.Contains(collidedObject)) {
            return;
        }

        // 대미지 주기
        if (collidedObject.CompareTag("Enemy")) {
            // 배율 적용
            float damage = weaponDamage;
            
            collidedObject.GetComponent<knight>().Hit(damage);
            collidedObjects.Add(collidedObject);
            StartCoroutine(AttackWaitCoroutine(1f, collidedObject));
        }
    }

    IEnumerator AttackWaitCoroutine(float waitDuration, GameObject collidedObject)
    {
        yield return new WaitForSeconds(waitDuration);
        collidedObjects.Remove(collidedObject);
    }
}
