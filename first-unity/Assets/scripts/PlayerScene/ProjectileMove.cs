using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public Vector3 launchDirection; // �߻� ����

    private void FixedUpdate()
    {
        float moveAmount = 10 * Time.fixedDeltaTime; // �̵� �ӵ� ����
        transform.Translate(launchDirection * moveAmount); // Translate�� �̵�
    }

    private void OnCollisionEnter(Collision collision) // �浹�� �Ͼ�� ���
    {
        Debug.Log(collision.gameObject.name);

        // Tag ���� Object�� ���
        if (collision.gameObject.tag == "Object") 
        {
            Destroy(this.gameObject); // �߻�ü ����
        }
        // Tag ���� Monster�� ���
        if (collision.gameObject.tag == "Monster")
        {
            Destroy(this.gameObject); // �߻�ü ����
            collision.gameObject.GetComponent<Monster>().Damaged(1);
        }
    }

    // Ʈ���� ���� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster")) // Tag�� �˻��Ѵ�.
        {
            Destroy(this.gameObject); // �߻�ü ����
            other.gameObject.GetComponent<Monster>().Damaged(1);
        }
    }
}
