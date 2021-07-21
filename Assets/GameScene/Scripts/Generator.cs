using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 蓮の葉を生成する
/// </summary>
public class Generator : MonoBehaviour
{

    [Tooltip("フィールド")]
    [SerializeField] FieldManager m_fieldManager = default;

    [Tooltip("生成オブジェクト")]
    [SerializeField] GameObject m_genOb;

    //  初期生成数
    [Tooltip("初期生成数上限")]
    [SerializeField] int m_upperLimit;
    [Tooltip("初期生成数下限")]
    [SerializeField] int m_lowerLimit;
    //

    //  m_erea m_gemObの生成範囲
    //  (X1,Y1)と(X2,Y2)を対角の頂点とする長方形
    float m_ereaRX;
    float m_ereaLX;
    float m_ereaTY;
    float m_ereaUY;
    //

    //  生成確立関連
    [Tooltip("生成確立")]
    [SerializeField] float m_Probability;
    [Tooltip("最低保証秒数")]
    [SerializeField] int m_minimumTime;
    /// <summary>連続非生成秒数</summary>
    float m_notGenerated = 0;
    /// <summary>前回抽選からの秒数</summary>
    float lastLottery = 0;
    //

    //  抽選間隔


    private void Start()
    {
        m_ereaRX = m_fieldManager.FieldEreaRX - 1;
        m_ereaLX = m_fieldManager.FieldEreaLX + 1;
        m_ereaTY = m_fieldManager.FieldEreaTY - 1;
        m_ereaUY = m_fieldManager.Position.y;

        for (int i = 0; i < Random.Range(m_lowerLimit, m_upperLimit); i++)  //Limitで指定された数だけm_genObを生成
        {
            Instantiate(m_genOb, new Vector3(Random.Range(m_ereaRX, m_ereaLX), Random.Range(m_ereaTY, m_ereaUY)), Quaternion.Euler(0, 0, 0));
        }
    }
    // Update is called once per frame
    void Update()
    {
        lastLottery += Time.deltaTime;

        if (lastLottery > 0.5)
        {
            //  生成部分
            if (Random.Range(0f, 100) < m_Probability)   //1フレーム毎にm_probability%の確率でm_genObをm_erea内に生成
            {
                Instantiate(m_genOb, new Vector3(Random.Range(m_ereaRX, m_ereaLX), m_ereaTY), Quaternion.Euler(0, 0, 0));
                m_notGenerated = 0; //生成したらrelifをリセット
            }
            else
            {
                m_notGenerated += lastLottery;   //生成しなかったフレームはrelifを＋１
            }
            if (m_notGenerated >= m_minimumTime)    //relifがm_timeを超えたらm_genObを生成し、relifをリセット
            {
                Instantiate(m_genOb, new Vector3(Random.Range(m_ereaRX, m_ereaLX), m_ereaTY), Quaternion.Euler(0, 0, 0));
                m_notGenerated = 0;
            }
            ///  生成部分
            lastLottery = 0;
        }
    }
}
