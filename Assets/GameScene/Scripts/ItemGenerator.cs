using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 蓮の葉を生成する
/// </summary>
public class ItemGenerator : MonoBehaviour
{

    public static Generator Instance { get; private set; }

    [Tooltip("フィールド")]
    [SerializeField] FieldManager m_fieldManager = default;

    [Tooltip("生成オブジェクト")]
    [SerializeField] GameObject m_genOb;



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
    /// <summary>前回抽選からの秒数</summary>
    float lastLottery = 0;
    //

    [Tooltip("抽選間隔")]
    [SerializeField] float m_lotteryInterval = default;

    [Tooltip("生成数上限")]
    [SerializeField] int m_maxInstanceNumber = default;

    [Tooltip("生成数上限時の対応")]
    [SerializeField] Coping m_coping = default;

    /// <summary>オブジェクトの生成モード</summary>
    State m_state = State.Nomal;

    List<GameObject> m_instanceObjects = new List<GameObject>();





    private void Start()
    {
        m_ereaRX = m_fieldManager.FieldEreaRX - 1;
        m_ereaLX = m_fieldManager.FieldEreaLX + 1;
        m_ereaTY = m_fieldManager.FieldEreaTY - 1;
        m_ereaUY = m_fieldManager.FieldEreaUY + 1;


    }
    // Update is called once per frame
    void Update()
    {
        lastLottery += Time.deltaTime;

        if (lastLottery > m_lotteryInterval)    //  一定間隔おきに抽選を行う
        {
            NullCheck();
            InstanseNumberCheck();
            Lottery();
            lastLottery = 0;
        }

    }

    /// <summary>
    /// 生成の抽選
    /// </summary>
    void Lottery()
    {
        if (Random.Range(0f, 100) < m_Probability && m_state != State.Stop)   //1フレーム毎にm_probability%の確率でm_genObをm_erea内に生成
        {
            PieGenerate();
        }
    }

    /// <summary>
    /// 消されたオブジェクトをリストから削除
    /// </summary>
    void NullCheck()
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

    /// <summary>
    /// 今実体化しているオブジェクトが規定内の個数であるかを確認し
    /// そうでなかった場合は決められた通りにstateを変更する。
    /// </summary>
    void InstanseNumberCheck()
    {
        if (m_instanceObjects.Count >= m_maxInstanceNumber)
        {
            if (m_coping == Coping.Destroy)
            {
                m_state = State.Desteoy;
            }
            else
            {
                m_state = State.Stop;
            }
        }
        else
        {
            m_state = State.Nomal;
        }
    }

    /// <summary>フィールド上のどこかにランダムで生成</summary>
    void PieGenerate()
    {
        Generate(Random.Range(m_ereaRX, m_ereaLX), Random.Range(m_ereaTY, m_ereaUY));
    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void Generate(float x, float y)
    {
        m_instanceObjects.Add(Instantiate(m_genOb, new Vector2(x, y), Quaternion.Euler(0, 0, 0)));
        if (m_state == State.Desteoy)
        {
            Destroy(m_instanceObjects[0], 0);
        }
    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    /// <param name="position"></param>
    void Generate(Vector2 position)
    {
        m_instanceObjects.Add(Instantiate(m_genOb, position, Quaternion.Euler(0, 0, 0)));
        if (m_state == State.Desteoy)
        {
            Destroy(m_instanceObjects[0], 0);
        }
    }


    enum Coping
    {
        /// <summary>古いものから消していく</summary>
        Destroy,
        /// <summary>生成を中断する</summary>
        Stop,
    }

    /// <summary>オブジェクトの生成モード</summary>
    enum State
    {
        Nomal,
        Stop,
        Desteoy,
    }
}
