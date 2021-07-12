using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    //  初めの葉っぱに乗っていられる制限時間(秒)
    [SerializeField] float m_time;
    [SerializeField] FrogController frogC = default;

    private void Start()
    {
       StartCoroutine("Break");
    }
    private void OnTriggerExit2D(Collider2D collision)  //プレイヤーが離れたら消去
    {
        if(collision.tag == "Frog")
        {
            Destroy(gameObject, 0);
        }
    }
    IEnumerator Break() //時限による葉っぱの破壊及びsinkをtrueに
    {
        yield return new WaitForSeconds(m_time);    //m_time経過後次の行へ



        ////  sinkを利用するためFrogControllerを取得
        //var frog = GameObject.Find("Frog");
        //var frogC = frog.GetComponent<FrogController>();
        ////

        frogC.Sink();
        Destroy(gameObject, 0); //このオブジェクトを消去
        yield break;
    }
}
