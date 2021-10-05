using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 蓮の葉を生成する
/// </summary>
public class Generator : MonoBehaviour
{

    public static Generator Instance { get; private set; }

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
    float m_centerY;
    //

    //  生成確立関連
    [Tooltip("生成確立")]
    [SerializeField] float m_Probability;
    [Tooltip("最低保証エリア")]
    [SerializeField] float m_passportErea;
    /// <summary>連続非生成秒数</summary>
    float m_notGenerated = 0;
    /// <summary>前回抽選からの秒数</summary>
    float lastLottery = 0;
    //

    [Tooltip("抽選間隔")]
    [SerializeField] float m_lotteryInterval = default;

    /// <summary>現在存在する蓮の葉</summary>
    List<GameObject> m_instanceObjects = new List<GameObject>();

    /// <summary>オブジェクトを生成するか否か</summary>
    bool m_play = false;





    //private void Start()
    //{
    //    SetUp();
    //}
    // Update is called once per frame
    void Update()
    {
        LostObjectCheck();

        if (m_play) //  生成が許可されている時のみ機能させる
        {
            lastLottery += Time.deltaTime;

            if (m_instanceObjects.Count > 0)
            {
                bool beingness = false;
                foreach (var ob in m_instanceObjects)   //  一定エリアより上に蓮が無くなったら湧かす
                {
                    if (ob.transform.position.y > m_fieldManager.transform.position.y - m_passportErea)
                    {
                        beingness = true;
                        break;
                    }
                }
                if (!beingness)
                {
                    TopGenerate();
                }
            }

            if (lastLottery > m_lotteryInterval && m_instanceObjects.Count < m_upperLimit)    //  一定間隔おきで一定個数以下の時抽選
            {
                Lottery();
                lastLottery = 0;
            }
        }

    }

    /// <summary>蓮の葉を全消去</summary>
    public void Destroy()
    {
        foreach (var go in m_instanceObjects)
            Destroy(go, 0);
    }

    /// <summary>
    /// 生成の抽選
    /// </summary>
    void Lottery()
    {
        if (Random.Range(0f, 100) < m_Probability)   //1フレーム毎にm_probability%の確率でm_genObをm_erea内に生成
        {
            TopGenerate();
            m_notGenerated = 0; //生成したらrelifをリセット
        }
        //else
        //{
        //    m_notGenerated += lastLottery;   //生成しなかったフレームはrelifを＋１
        //}
        //if (m_notGenerated >= m_minimumTime)    //relifがm_timeを超えたらm_genObを生成し、relifをリセット
        //{
        //    TopGenerate();
        //    m_notGenerated = 0;
        //}
    }


    /// <summary>
    /// limit内の回数上半分に生成
    /// </summary>
    public void SetUp()
    {
        m_ereaRX = m_fieldManager.FieldEreaRX - 1;
        m_ereaLX = m_fieldManager.FieldEreaLX + 1;
        m_ereaTY = m_fieldManager.FieldEreaTY - 1;
        m_centerY = m_fieldManager.Position.y;

        Active(true);

        for (int i = 0; i < Random.Range(m_lowerLimit, m_upperLimit); i++)  //Limitで指定された数だけgenObを生成
        {
            UpeerHlafGenerate();
        }

    }

    /// <summary>消されたオブジェクトをリストから削除</summary>
    void LostObjectCheck()
    {
        for (int i = 0; i < m_instanceObjects.Count; i++)
        {
            if (m_instanceObjects[i] == null)
            {
                m_instanceObjects.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>生成機能のオンオフ切り替え</summary>
    /// <param name="value"></param>
    public void Active(bool value)
    {
        m_play = value;
    }

    /// <summary>上半分のどこかにランダムで生成</summary>
    void UpeerHlafGenerate()
    {
        Generate(Random.Range(m_ereaRX, m_ereaLX), Random.Range(m_ereaTY, m_centerY));
    }

    /// <summary>頂点のどこかにランダムで生成</summary>
    void TopGenerate()
    {
        Generate(Random.Range(m_ereaRX, m_ereaLX), m_ereaTY);
    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void Generate(float x, float y)
    {
        m_instanceObjects.Add(Instantiate(m_genOb, new Vector2(x, y), Quaternion.Euler(0, 0, 0)));
    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    /// <param name="position"></param>
    void Generate(Vector2 position)
    {
        m_instanceObjects.Add(Instantiate(m_genOb, position, Quaternion.Euler(0, 0, 0)));
    }
}
