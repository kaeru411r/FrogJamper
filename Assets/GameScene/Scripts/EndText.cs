using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>プレイ終了時の表示を司るコンポーネント</summary>
public class EndText : MonoBehaviour
{
    [Tooltip("プレイヤーコントロールクラス")]
    [SerializeField] FrogController m_frog;
    [Tooltip("スコア管理クラス")]
    [SerializeField] Score m_scoreMaanger;
    [Tooltip("ゲームマネージャー")]
    [SerializeField] GameManager3 m_gameManager3;

    [Tooltip("ゲームオーバーとクリア時の表示の高さ")]
    [SerializeField] float m_posY;
    [Tooltip("ゲームオーバーとクリア時の表示大きさ")]
    [SerializeField] int m_size;
    [Tooltip("スコアの上昇にかける時間")]
    [SerializeField] float m_changeInterval;
    /// <summary>表示スコア</summary>
    int m_score;
    /// <summary>テキスト</summary>
    Text m_text;


    public void Display()
    {
        gameObject.SetActive(true);     //  表示をオンに
        m_text = GetComponent<Text>();

        if (m_gameManager3.StateGet() == 2)
        {
            DeadText(m_scoreMaanger.ScoreGet);
            //  残機と今ターンでのスコア表示
        }
        else if(m_gameManager3.StateGet() == 3)
        {
            End("Game  Ovar\nScore  " + 0);
            GameOverText(m_scoreMaanger.ScoreGet);

        }
        else if(m_gameManager3.StateGet() == 4)
        {
            End("Game  Clear");

        }
    }

    void DeadText(int value)
    {
        int tempScore = m_scoreMaanger.ScoreGet;
        DOTween.To(() => m_score, x =>
        {
            m_score = x;
            m_text.text = $"Life {m_frog.LIfe}\n{m_score}";
        }, m_scoreMaanger.ScoreGet, m_changeInterval);
    }

    void GameOverText(int value)
    {
        int tempScore = m_scoreMaanger.ScoreGet;
        DOTween.To(() => m_score, x =>
        {
            m_score = x;
            m_text.text = $"Game  Ovar\nScore  {m_score}";
        }, m_scoreMaanger.ScoreGet, m_changeInterval);
    }

    /// <summary>リプレイしないときに呼ぶ
    /// 画面上に表示が出る</summary>
    /// <param name="str"></param>
    void End(string str)
    {
        RectTransform rT = gameObject.GetComponent<RectTransform>();
        rT.position = new Vector2(rT.position.x, rT.position.y + m_posY);

        Text text = gameObject.GetComponent<Text>();

        text.text = str;
        text.fontSize = m_size;
    }
}
