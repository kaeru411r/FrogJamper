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
    string m_scoreText;
    public string ScoreText { get { return m_scoreText; } }

    /// <summary>各プレイのスコアを記録</summary>
    static List<int> m_scores = new List<int>();

    /// <summary>timeの端数を記録</summary>
    float m_fractionTime = 0;

    /// <summary>現在ゲームが止まっているか</summary>
    bool m_stop = false;

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

    private void Start()
    {
        m_text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_stop)
        {
            //  スコア計算部分
            int iTime = (int)(Time.deltaTime * 100);                    //  Time.deltaTimeを100倍した数の整数部分でiTimeを初期化
            m_fractionTime += Time.deltaTime - iTime / 100f;            //  iTimeに代入した分を除いたTime.deltaTimeをm_fractionTimeに足す
            iTime += (int)(m_fractionTime * 100);                       //  m_fractionTimeを100倍した数の整数部分をiTimeに足す
            m_fractionTime -= ((int)(m_fractionTime * 100)) / 100f;     //  m_fractionTimeからiTimeに足した分を引く
            m_score += iTime;                                           //  m_socreにiTimeを足す
            //

            string text = "score " + m_score + m_bonusText;
            m_text.text = text;    //scoreを表示
        }
    }

    public void Stop()
    {
        m_stop = !m_stop;
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
    }

    /// <summary>
    /// 前回記録時から今までのスコアを記録
    /// </summary>
    public void ScoreRecode()
    {
        int lastScore = 0;
        foreach (var buf in m_scores)
        {
            lastScore += buf;
        }
        m_scoreText = "This turn score" + (m_score - lastScore);
        m_scores.Add(m_score - lastScore);
        Debug.Log(m_score);
        foreach (var buf in m_scores)
        {
            Debug.Log(buf);
        }
    }

    /// <summary>
    /// スコアボーナスを獲得する
    /// </summary>
    public void AddScore()
    {
        m_score += m_bonusScore;
        StartCoroutine(BonusDisplay());
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
}
