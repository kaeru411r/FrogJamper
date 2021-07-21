using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// スコアの管理、表示をする。
/// </summary>
public class Score : MonoBehaviour
{
    [Tooltip("テキスト本体")]
    [SerializeField] Text text = default;

    //  スコアを記録
    static int score = 0;

    //  各プレイのスコアを記録
    static List<int> scores = new List<int>();

    //  timeの端数を記録
    float fTime = 0;

    //  現在ゲームが止まっているか
    bool stop = false;




    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            //  スコア計算部分
            fTime += Time.deltaTime;            //1フレームの時間fTimeを取得
            int iTime = (int)(fTime * 100);     //iTimeにfTimeを100倍した整数部分を代入
            fTime -= iTime / 100f;              //iTimeに代入した分をfTimeから引く
            score += iTime;                     //iTimeをscoreに加算
            //

            text.text = "score " + score;    //scoreを表示
        }
    }

    public void Stop()
    {
        stop = !stop;
    }

    public void Stop(bool tf)
    {
        if (tf)
        {
            stop = true;
        }
        else
        {
            stop = false;
        }
    }

    /// <summary>
    /// スコアを0にする
    /// スコアリストをリセットする
    /// </summary>
    public void ScoreReset()
    {
        score = 0;
        scores.Clear();
    }

    /// <summary>
    /// 前回記録時から今までのスコアを記録
    /// </summary>
    public void ScoreRecode()
    {
        int lastScore = 0;
        foreach (var buf in scores)
        {
            lastScore += buf;
        }
        scores.Add(score - lastScore);
        Debug.Log(score);
        foreach (var buf in scores)
        {
            Debug.Log(buf);
        }
    }
}
