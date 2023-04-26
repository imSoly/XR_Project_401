using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public enum PROJECTILETYPE // enum���� Ÿ�� ����
    {
        PLAYER, 
        MONSTER
    }
    public Vector3 launchDirection; // �߻� ����

    public PROJECTILETYPE projectileType;

    private void FixedUpdate()
    {
        float moveAmount = 10 * Time.fixedDeltaTime; // �̵� �ӵ� ����
        transform.Translate(launchDirection * moveAmount); // Translate�� �̵�
    }

    // private void OnCollisionEnter(Collision collision) // �浹�� �Ͼ�� ���
    //  {
    // Debug.Log(collision.gameObject.name);
    // 
    //      // Tag ���� Object�� ���
    //      if (collision.gameObject.tag == "Object") 
    //   {
    // Destroy(this.gameObject); // �߻�ü ����
    // }
    // // Tag ���� Monster�� ���
    // if (collision.gameObject.tag == "Monster")
    // {
    // Destroy(this.gameObject); // �߻�ü ����
    // collision.gameObject.GetComponent<Monster>().Damaged(1);
    // }
    // }

    // Ʈ���� ���� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && projectileType == PROJECTILETYPE.PLAYER)  // Tag�� �˻��Ѵ�.
        {
            Destroy(this.gameObject);
            other.gameObject.GetComponent<Monster>().Damaged(1);
            GameObject Temp = GameObject.FindGameObjectWithTag("GameManager");
            Temp.GetComponent<HUDCharacterManager>().UpdateHUDText(
                "1", other.gameObject, new Vector3(0.0f, 10.0f, 0.0f));
        }

        if (other.CompareTag("Player") && projectileType == PROJECTILETYPE.MONSTER)  // Tag�� �˻��Ѵ�.
        {
            Destroy(this.gameObject);
            other.gameObject.GetComponent<PlayerHp>().Damaged(1);
            GameObject Temp = GameObject.FindGameObjectWithTag("GameManager");
            Temp.GetComponent<HUDCharacterManager>().UpdateHUDText(
                "1", other.gameObject, new Vector3(0.0f, 10.0f, 0.0f));
        }
    }
}
