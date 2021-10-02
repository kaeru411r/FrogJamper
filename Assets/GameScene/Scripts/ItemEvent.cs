using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]

public class ItemEvent : MonoBehaviour
{
    [Tooltip("アイテムの流れる速さの上限")]
    [SerializeField] float m_speedUpperLimit = default;
    [Tooltip("アイテムの流れる速さの下限")]
    [SerializeField] float m_speedLowerLimit = default;

    [SerializeField] UnityEvent m_useEvent;

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

    private void OnTriggerEnter(Collider other)
    {
        Use();
    }

    void Use()
    {
        m_useEvent.Invoke();
        Destroy();
    }
    void Destroy()
    {
        Destroy(gameObject, 0);
    }

    /// <summary>
    /// プレイヤーのライフをvalue分追加
    /// </summary>
    /// <param name="value"></param>
    public void AddLife(int value)
    {
        GameObject.Find("Frog").GetComponent<FrogController>().AddLife(value);   //  プレイヤーのライフをvalue分を足す
    }

    /// <summary>
    /// スコアをvalue分追加
    /// </summary>
    /// <param name="value"></param>
    public void AddScore(int value)
    {
        GameObject.Find("ScoreBord").GetComponent<Score>().AddScore(value); //  スコアをvalue分追加
    }

    /// <summary>
    /// プレイヤーが沈む
    /// </summary>
    public void Crash()
    {
        GameObject.Find("Frog").GetComponent<FrogController>().Crash(); //  溺れる
    }
}
