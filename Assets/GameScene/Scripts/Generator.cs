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

    //  SetUp時の生成数
    [Tooltip("SetUp時の初期生成数上限")]
    [SerializeField] int m_upperLimit;
    [Tooltip("SetUp時の初期生成数下限")]
    [SerializeField] int m_lowerLimit;
    //


    //  m_erea m_gemObの生成範囲
    //  (X1,Y1)と(X2,Y2)を対角の頂点とする長方形
    float m_ereaRX;
    float m_ereaLX;
    float m_ereaTY;
    float m_ereaUY;
    float m_centerY;
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

    [Tooltip("抽選間隔")]
    [SerializeField] float m_lotteryInterval = default;

    [SerializeField] Type m_type = default;

    [SerializeField] bool m_start = default;


    private void Start()
    {
        m_ereaRX = m_fieldManager.FieldEreaRX - 1;
        m_ereaLX = m_fieldManager.FieldEreaLX + 1;
        m_ereaTY = m_fieldManager.FieldEreaTY - 1;
        m_ereaUY = m_fieldManager.FieldEreaUY + 1;
        m_centerY = m_fieldManager.Position.y;

        if (m_start)
        {
            SetUp();
        }

    }
    // Update is called once per frame
    void Update()
    {
        lastLottery += Time.deltaTime;

        if (lastLottery > m_lotteryInterval)
        {
            Lottery();
            lastLottery = 0;
        }

    }

    /// <summary>
    /// 生成の抽選
    /// </summary>
    void Lottery()
    {
        if (Random.Range(0f, 100) < m_Probability)   //1フレーム毎にm_probability%の確率でm_genObをm_erea内に生成
        {
            if (m_type == Type.Lotus)
            {
                TopGenerate();
            }
            else if (m_type == Type.Item)
            {
                PieGenerate();
            }
            m_notGenerated = 0; //生成したらrelifをリセット
        }
        else
        {
            m_notGenerated += lastLottery;   //生成しなかったフレームはrelifを＋１
        }
        if (m_notGenerated >= m_minimumTime)    //relifがm_timeを超えたらm_genObを生成し、relifをリセット
        {
            if (m_type == Type.Lotus)
            {
                TopGenerate();
            }
            else if(m_type == Type.Item)
            {
                PieGenerate();
            }
            m_notGenerated = 0;
        }
    }



    /// <summary>
    /// limit内の回数上半分に生成
    /// </summary>
    public void SetUp()
    {
        for (int i = 0; i < Random.Range(m_lowerLimit, m_upperLimit); i++)  //Limitで指定された数だけgenObを生成
        {
            UpeerHlafGenerate();
        }

    }

    /// <summary>上半分のどこかにランダムで生成</summary>
    void UpeerHlafGenerate()
    {
        Instantiate(m_genOb, new Vector3(Random.Range(m_ereaRX, m_ereaLX), Random.Range(m_ereaTY, m_centerY)), Quaternion.Euler(0, 0, 0));
    }

    /// <summary>頂点のどこかにランダムで生成</summary>
    void TopGenerate()
    {
        Instantiate(m_genOb, new Vector3(Random.Range(m_ereaRX, m_ereaLX), m_ereaTY), Quaternion.Euler(0, 0, 0));
    }

    /// <summary>フィールド上のどこかにランダムで生成</summary>
    void PieGenerate()
    {
        Instantiate(m_genOb, new Vector3(Random.Range(m_ereaRX, m_ereaLX), Random.Range(m_ereaTY, m_ereaUY)), Quaternion.Euler(0, 0, 0));
    }

    public enum Type
    {
        Lotus,
        Item,
    }
}
