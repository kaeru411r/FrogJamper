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

    /// <summary>各プレイのスコアを記録</summary>
    static List<int> m_scores = new List<int>();

    /// <summary>timeの端数を記録</summary>
    float m_fractionTime = 0;

    /// <summary>現在ゲームが止まっているか</summary>
    bool m_stop = false;

    [Tooltip("BonusScoreで足されるスコア")]
    [SerializeField] int m_bonusScore = default;

    /// <summary>ボーナススコア表示に使用する文末の文字列</summary>
    string m_bonusText = " ";
    /// <summary>ボーナススコア表示に使用する文末の文字列の数値</summary>
    int m_bonusTextValue = 0;

    [Tooltip("ボーナススコアの表示時間")]
    [SerializeField] float m_bonusDisplayTime = default;
    /// <summary>ボーナススコア表示中か否か</summary>
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
            m_fractionTime += Time.deltaTime;            //1フレームの時間fTimeを取得
            int iTime = (int)(m_fractionTime * 100);     //iTimeにfTimeを100倍した整数部分を代入
            m_fractionTime -= iTime / 100f;              //iTimeに代入した分をfTimeから引く
            m_score += iTime;                            //iTimeをscoreに加算
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
    /// </summary>
    IEnumerator BonusDisplay()
    {
        if(m_bonusTextValue != 0)
        {
            Debug.LogWarning("0" + n);
            m_bonusTextValue += m_bonusScore;
            yield break;
        }
        else
        {
            Debug.LogWarning("2" + n);
            m_bonusTextValue += m_bonusScore;
            yield return null;
        }
        m_bonusText = "  +" + m_bonusTextValue;
        m_bonusTextValue = 0;
        Debug.LogWarning("1" + n);
        m_bonusDisplayNumber++;
        yield return new WaitForSeconds(m_bonusDisplayTime);
        m_bonusDisplayNumber--;
        if (m_bonusDisplayNumber <= 0)
        {
            m_bonusText = " ";
        }
    }
}
