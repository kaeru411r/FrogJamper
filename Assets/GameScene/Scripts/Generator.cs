using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    //  生成オブジェクト
    [SerializeField] GameObject m_genOb;

    //  初期生成数
    [SerializeField] int m_upperLimit;
    [SerializeField] int m_lowerLimit;
    //

    //  m_erea m_gemObの生成範囲
    //  (X1,Y1)と(X2,Y2)を対角の頂点とする長方形
    [SerializeField] float m_ereaX1;
    [SerializeField] float m_ereaX2;
    [SerializeField] float m_ereaY1;
    [SerializeField] float m_ereaY2;
    //

    //  生成確立関連
    [SerializeField] float m_Probability;   //  生成確立
    [SerializeField] int m_time;            //  最低保証秒数
    float relief = 0;                       //  連続非生成秒数
    float time = 0;                         //  前回抽選からの秒数
    //

    //  抽選間隔


    private void Start()
    {

        for (int i = 0; i < Random.Range(m_lowerLimit, m_upperLimit); i++)  //Limitで指定された数だけm_genObを生成
        {
            Instantiate(m_genOb, new Vector3(Random.Range(m_ereaX1, m_ereaX2), Random.Range(m_ereaY1, m_ereaY2)), Quaternion.Euler(0, 0, 0));
        }
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > 0.5)
        {
            //  生成部分
            if (Random.Range(0f, 100) < m_Probability)   //1フレーム毎にm_probability%の確率でm_genObをm_erea内に生成
            {
                Instantiate(m_genOb, new Vector3(Random.Range(m_ereaX1, m_ereaX2), m_ereaY1), Quaternion.Euler(0, 0, 0));
                relief = 0; //生成したらrelifをリセット
            }
            else
            {
                relief += time;   //生成しなかったフレームはrelifを＋１
            }
            if (relief >= m_time)    //relifがm_timeを超えたらm_genObを生成し、relifをリセット
            {
                Instantiate(m_genOb, new Vector3(Random.Range(m_ereaX1, m_ereaX2), m_ereaY1), Quaternion.Euler(0, 0, 0));
                relief = 0;
            }
            ///  生成部分
            time = 0;
        }
    }
}
