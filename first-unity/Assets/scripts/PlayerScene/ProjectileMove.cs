using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public Vector3 launchDirection; // 발사 방향

    private void FixedUpdate()
    {
        float moveAmount = 10 * Time.fixedDeltaTime; // 이동 속도 설정
        transform.Translate(launchDirection * moveAmount); // Translate로 이동
    }

    private void OnCollisionEnter(Collision collision) // 충돌이 일어났을 경우
    {
        Debug.Log(collision.gameObject.name);

        // Tag 값이 Object인 경우
        if (collision.gameObject.tag == "Object") 
        {
            Destroy(this.gameObject); // 발사체 삭제
        }
        // Tag 값이 Monster인 경우
        if (collision.gameObject.tag == "Monster")
        {
            Destroy(this.gameObject); // 발사체 삭제
            collision.gameObject.GetComponent<Monster>().Damaged(1);
        }
    }

    // 트리거 예약 함수
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster")) // Tag를 검사한다.
        {
            Destroy(this.gameObject); // 발사체 삭제
            other.gameObject.GetComponent<Monster>().Damaged(1);
        }
    }
}
