using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// スコアの管理、表示をする。
/// </summary>
public class Score : MonoBehaviour
{
    /// <summary>テキスト本体</summary>
    Text m_text = default;

    /// <summary>スコアを記録</summary>
    static int m_score = 0;
    public int ScoreGet { get { return m_score; } }

    string m_scoreText;
    public string ScoreText { get { return m_scoreText; } }

    /// <summary>各プレイのスコアを記録</summary>
    static List<int> m_scores = new List<int>();

    /// <summary>timeの端数を記録</summary>
    float m_fractionTime = 0;

    /// <summary>現在ゲームが止まっているか</summary>
    bool m_stop = true;

    [Tooltip("BonusScoreで足されるスコア")]
    [SerializeField] int m_bonusScore = default;

    /// <summary>ボーナススコア表示に使用する文末の文字列</summary>
    string m_bonusText = "";
    /// <summary>ボーナススコア表示に使用する文末の文字列の数値</summary>
    int m_bonusTextValue = 0;

    [Tooltip("ボーナススコアの表示時間")]
    [SerializeField] float m_bonusDisplayTime = default;
    /// <summary>動作中のボーナススコア表示関数の数</summary>
    int m_bonusDisplayNumber = 0;

    [Tooltip("クリアに必要なスコア")]
    [SerializeField] int m_goal;

    public int Goal { get { return m_goal; } }

    [Tooltip("ゲームマネージャー")]
    [SerializeField] GameManager3 m_gameManager;

    [Tooltip("スコアの獲得量の係数")]
    [SerializeField] float m_coefficient;

    /// <summary>実際に使用するスコアの獲得量の係数</summary>
    static float m_staticCoefficient;



    private void Start()
    {
        m_text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_stop)
        {
            if (m_score >= 1000000000)  //  スコアがint型の限界を超えないように
            {
                m_score = 1000000000;
            }

            //  スコア計算部分
            int iTime = (int)(Time.deltaTime * 100 * m_staticCoefficient);                    //  Time.deltaTimeを100倍した数の整数部分でiTimeを初期化
            m_fractionTime += Time.deltaTime - iTime / 100f / m_staticCoefficient;            //  iTimeに代入した分を除いたTime.deltaTimeをm_fractionTimeに足す
            iTime += (int)(m_fractionTime * 100 * m_staticCoefficient);                       //  m_fractionTimeを100倍した数の整数部分をiTimeに足す
            m_fractionTime -= ((int)(m_fractionTime * 100)) / 100f / m_staticCoefficient;     //  m_fractionTimeからiTimeに足した分を引く
            m_score += iTime;                                           //  m_socreにiTimeを足す
            //

            string text = "score " + m_score + m_bonusText;
            m_text.text = text;    //scoreを表示
        }

        if(m_score >= m_goal && !m_stop)    //  ゲームクリア判定
        {
            m_gameManager.GameClear();
        }

    }


    public void Stop(bool tf)
    {
        if (tf)
        {
            m_stop = true;
        }
        else
        {
            m_stop = false;
        }
    }

    /// <summary>
    /// スコアを0にする
    /// スコアリストをリセットする
    /// </summary>
    public void ScoreReset()
    {
        m_score = 0;
        m_scores.Clear();
        m_staticCoefficient = m_coefficient;
    }

    /// <summary>
    /// 前回記録時から今までのスコアを記録
    /// </summary>
    public void ScoreRecode()
    {
        int lastScore = 0;
        //foreach (var buf in m_scores)
        //{
        //    lastScore += buf;
        //}
        m_scores.ForEach(i => lastScore += i);
        m_scoreText = "This turn score" + (m_score - lastScore);    //  死亡時テキスト設定
        m_scores.Add(m_score - lastScore);                          //  今世のスコアを記録
        Debug.Log(m_score);
        //foreach (var buf in m_scores)
        //{
        //    Debug.Log(buf);
        //}
        m_scores.ForEach(i => Debug.Log(i));
    }

    /// <summary>
    /// スコアボーナスを獲得する
    /// </summary>
    public void AddScore()
    {
        m_score += m_bonusScore;
        StartCoroutine(BonusDisplay());
    }

    /// <summary>係数を設定する</summary>
    /// <param name="value"></param>
    public void SetCoefficient(float value)
    {
        m_staticCoefficient = value;
    }

    /// <summary>係数を現在のvalue倍にする</summary>
    /// <param name="value"></param>
    public void MultiplyCoefficient(float value)
    {
        m_staticCoefficient *= value;
    }

    /// <summary>
    /// スコアボーナスを獲得する
    /// </summary>
    public void AddScore(int value)
    {
        m_score += value;
        StartCoroutine(BonusDisplay(value));
    }

    /// <summary>
    /// スコアボーナスを表示
    /// 一フレームに呼ばれた回数分だけボーナスの表示数値が倍増していく
    /// </summary>
    IEnumerator BonusDisplay()
    {
        if(m_bonusTextValue != 0)
        {
            m_bonusTextValue += m_bonusScore;
            yield break;                        //  2回目以降は処理をここで終了する
        }
        else
        {
            m_bonusTextValue += m_bonusScore;
            yield return null;                  //  1回目のみ次のフレームに処理を持ち越す
        }
        //  ここまで1フレーム目

        m_bonusText = "  +" + m_bonusTextValue;
        m_bonusTextValue = 0;
        m_bonusDisplayNumber++;
        //  ここまで2フレーム目

        yield return new WaitForSeconds(m_bonusDisplayTime);

        //  ここから指定秒数経過後
        m_bonusDisplayNumber--;
        if (m_bonusDisplayNumber <= 0)          //  もしこのインスタンス以降にインスタンス化されたこの関数が無ければ表示を消す
        {
            m_bonusText = "";
        }
    }
    /// <summary>
    /// スコアボーナスを表示
    /// 一フレームに呼ばれた回数分だけボーナスの表示数値が倍増していく
    /// </summary>
    IEnumerator BonusDisplay(int value)
    {
        if (m_bonusTextValue != 0)
        {
            m_bonusTextValue += value;
            yield break;                        //  2回目以降は処理をここで終了する
        }
        else
        {
            m_bonusTextValue += value;
            yield return null;                  //  1回目のみ次のフレームに処理を持ち越す
        }
        //  ここまで1フレーム目

        m_bonusText = "  +" + m_bonusTextValue;
        m_bonusTextValue = 0;
        m_bonusDisplayNumber++;
        //  ここまで2フレーム目

        yield return new WaitForSeconds(m_bonusDisplayTime);

        //  ここから指定秒数経過後
        m_bonusDisplayNumber--;
        if (m_bonusDisplayNumber <= 0)          //  もしこのインスタンス以降にインスタンス化されたこの関数が無ければ表示を消す
        {
            m_bonusText = "";
        }
    }

    /// <summary>死亡時に行う処理</summary>
    public void End()
    {


        //  

        Stop(true);
        ScoreRecode();

    }
}
