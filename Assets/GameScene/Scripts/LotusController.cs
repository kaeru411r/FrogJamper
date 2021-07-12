using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusController : MonoBehaviour
{
    //  オブジェクトの移動速度の上下限
    [SerializeField] float m_upperLimit = default;
    [SerializeField] float m_lowerLimit = default;
    //

    //  オブジェクトの存在限界範囲
    [SerializeField] float m_existenceLimit;

    //オブジェクトの移動速度
     float speed = default;

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody2D>();

        //オブジェクトの移動速度を決定
        speed = Random.Range(m_lowerLimit, m_upperLimit);

        //オブジェクトの移動速度を変更
        rb.velocity = Vector2.up * -speed;
    }

    private void Update()   //オブジェクトが指定範囲から出たら消去
    {
        if(transform.position.y < m_existenceLimit)
        {
            Destroy(gameObject, 0);
        }
    }
}
