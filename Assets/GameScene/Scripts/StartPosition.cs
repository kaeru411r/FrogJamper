using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤーが最初に乗っている蓮の葉のコントロールをする
/// </summary>
public class StartPosition : MonoBehaviour
{
    [Tooltip("初めの葉っぱに乗っていられる制限時間(秒)")]
    [SerializeField] float m_time;

    [Tooltip("FrogControllerコンポーネント")]
    [SerializeField] FrogController m_frogController = default;

    [Tooltip("FieldManagerコンポーネント")]
    [SerializeField] FieldManager m_fieldManager = default;

    [Tooltip("点滅している時間")]
    [SerializeField] float m_blinkTime;

    private void Start()
    {
        transform.position = new Vector3(m_fieldManager.Position.x, m_fieldManager.FieldEreaUY + 1);
        m_frogController.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    /// <summary>
    /// 消滅までのカウントダウンを開始する
    /// </summary>
    public void Timer()
    {
        StartCoroutine(Break());
    }

    private void OnTriggerExit2D(Collider2D collision)  //プレイヤーが離れたら消去
    {
        if (collision.tag == "Frog")
        {
            Destroy(gameObject, 0);
        }
    }
    IEnumerator Break() //時限による葉っぱの破壊及びsinkを呼び出し
    {
        yield return new WaitForSeconds(m_time - m_blinkTime);    //m_time経過後次の行へ

        GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(m_blinkTime);


        Debug.Log(" ");
        m_frogController.SinkStart();
        Destroy(gameObject, 0); //このオブジェクトを消去
    }
}
