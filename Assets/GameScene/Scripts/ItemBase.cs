using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]

public class ItemBase : MonoBehaviour
{

    [Tooltip("アイテムの流れる速さの上限")]
    [SerializeField] float m_speedUpperLimit = default;
    [Tooltip("アイテムの流れる速さの下限")]
    [SerializeField] float m_speedLowerLimit = default;

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody2D>();

        //  オブジェクトの物理的挙動をなくす
        rb.bodyType = RigidbodyType2D.Kinematic;

        //オブジェクトの移動速度を決定
        var speed = Random.Range(m_speedLowerLimit, m_speedUpperLimit);

        //オブジェクトの移動速度を変更
        rb.velocity = Vector2.up * -speed;

        //  オブジェクトの回転を無効化
        rb.freezeRotation = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Frog")
        {
            Use();
        }
    }

    public virtual void Use()
    {
        Debug.LogError("処理を上書きしてください");
    }
}
