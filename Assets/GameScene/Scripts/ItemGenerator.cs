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
    [SerializeField] List<GameObject> m_genOb = new List<GameObject>();

    [Tooltip("各アイテムの生成優先値")]
    [SerializeField] int[] m_hierarchy = default;
    [Tooltip("各アイテムの生成数上限")]
    [SerializeField] int[] m_maxInstanceNumbers = default;

    /// <summary>オブジェクトを生成するか否か</summary>
    bool m_play = false;



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
    float m_lastLottery = 0;
    //

    [Tooltip("抽選間隔")]
    [SerializeField] float m_lotteryInterval = default;


    [Tooltip("生成数上限時の対応")]
    [SerializeField] Coping m_coping = default;

    /// <summary>オブジェクトの生成モード</summary>
    List<State> m_states = new List<State>();

    /// <summary>実体化しているアイテム達</summary>
    List<List<GameObject>> m_instanceObjects = new List<List<GameObject>>();





    //private void Start()
    //{
    //    SetUp();
    //}


    // Update is called once per frame
    void Update()
    {
        if (m_play) //生成機能がオンの時のみ機能
        {
            m_lastLottery += Time.deltaTime;

            if (m_lastLottery > m_lotteryInterval)    //  一定間隔おきに抽選を行う
            {
                int index = ItemLottery();
                NullCheck();
                InstanseNumberCheck();
                Lottery(index);
                m_lastLottery = 0;
            }
        }

    }

    /// <summary>プレイ開始時に呼び出し</summary>
    public void SetUp()
    {
        Active(true);

        m_ereaRX = m_fieldManager.FieldEreaRX - 1;
        m_ereaLX = m_fieldManager.FieldEreaLX + 1;
        m_ereaTY = m_fieldManager.FieldEreaTY - 1;
        m_ereaUY = m_fieldManager.FieldEreaUY + 1;

        for (int i = 0; i < m_genOb.Count; i++)  //  オブジェクトの種類の数管理用リストを拡張
        {
            m_instanceObjects.Add(new List<GameObject>());
            m_states.Add(State.Nomal);
        }

    }

    /// <summary>
    /// 生成の抽選
    /// </summary>
    void Lottery(int index)
    {
        if (Random.Range(0f, 100) < m_Probability && m_states[index] != State.Stop)   //1フレーム毎にm_probability%の確率でm_genObをm_erea内に生成
        {
            PieGenerate(index);
        }
    }

    /// <summary>
    /// 消されたオブジェクトをリストから削除
    /// </summary>
    void NullCheck()
    {
            //  m_instanceObjectsの全ての要素をチェックし、nullがあればそれを消す
        for (int i = 0; i < m_instanceObjects.Count; i++)
        {
            for(int l = 0; l < m_instanceObjects[i].Count; l++)
            {
                if (m_instanceObjects[i][l] == null)
                {
                    m_instanceObjects[i].RemoveAt(l);
                    l--;
                }
            }
        }
    }

    /// <summary>
    /// 今実体化しているオブジェクトが規定内の個数であるかを確認し
    /// そうでなかった場合は決められた通りにstateを変更する。
    /// </summary>
    void InstanseNumberCheck()
    {
        for(int i = 0; i < m_instanceObjects.Count && i < m_maxInstanceNumbers.Length; i++)
        {
                //  もし実体化しているオブジェクトの数が規定値を超えていた場合、決められた通りにm_statesを変更する
            if (m_instanceObjects[i].Count >= m_maxInstanceNumbers[i])
            {
                if (m_coping == Coping.Destroy)
                {
                    m_states[i] = State.Desteoy;
                }
                else if(m_coping == Coping.Stop)
                {
                    m_states[i] = State.Stop;
                }
            }
            else
            {
                m_states[i] = State.Nomal;
            }
        }
    }

    /// <summary>フィールド上のどこかにランダムで生成</summary>
    void PieGenerate(int index)
    {
        Generate(Random.Range(m_ereaRX, m_ereaLX), Random.Range(m_ereaTY, m_ereaUY), index);
    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void Generate(float x, float y, int index)
    {
            //  アイテムを生成し、そのGameObjectをリストに格納する
        m_instanceObjects[index].Add(Instantiate(m_genOb[index], new Vector2(x, y), Quaternion.Euler(0, 0, 0)));
        if (m_states[index] == State.Desteoy)
        {
            Destroy(m_instanceObjects[index][0], 0);
        }

    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    /// <param name="position"></param>
    void Generate(Vector2 position, int index)
    {
        //  アイテムを生成し、そのGameObjectをリストに格納する
        m_instanceObjects[index].Add(Instantiate(m_genOb[index], position, Quaternion.Euler(0, 0, 0)));
        if (m_states[index] == State.Desteoy)
        {
            Destroy(m_instanceObjects[index][0], 0);
        }
    }

    /// <summary>
    /// アイテムの抽選を行う
    /// </summary>
    /// <returns>index</returns>
    int ItemLottery()
    {
        int index = 0;
        int sum = 0;
        for (int i = 0; i < m_genOb.Count && i < m_hierarchy.Length; i++)   // 各アイテムの生成優先度を合計する
        {
            sum += m_hierarchy[i];
        }
        int num = (int)Random.Range(0, sum);                                // その中からランダムにひとつ数値をとる
        sum = 0;
        for (int i = 0; i < m_genOb.Count && i < m_hierarchy.Length; i++)   // その数値が優先度の列のどこに当たるかによって返す値を決める
        {
            sum += m_hierarchy[i];
            if (num <= sum)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    /// <summary>アイテムを全て消す</summary>
    public void Destroy()
    {
        m_instanceObjects.ForEach(l => l.ForEach(go => Destroy(go)));
    }

    /// <summary>生成機能のオンオフ切り替え</summary>
    /// <param name="value"></param>
    public void Active(bool value)
    {
        m_play = value;
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
