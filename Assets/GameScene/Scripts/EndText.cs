using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>プレイ終了時の表示を司るコンポーネント</summary>
public class EndText : MonoBehaviour
{
    [Tooltip("プレイヤーコントロールクラス")]
    [SerializeField] FrogController m_frog;
    [Tooltip("スコア管理クラス")]
    [SerializeField] Score m_score;
    [Tooltip("ゲームマネージャー")]
    [SerializeField] GameManager3 m_gameManager3;

    [Tooltip("ゲームオーバーとクリア時の表示の高さ")]
    [SerializeField] float m_posY;
    [Tooltip("ゲームオーバーとクリア時の表示大きさ")]
    [SerializeField] int m_size;


    public void Display()
    {
        gameObject.SetActive(true);     //  表示をオンに

        if (m_gameManager3.StateGet() == 2)
        {

            //  残機と今ターンでのスコア表示
            gameObject.GetComponent<Text>().text = "Life " + m_frog.LIfe + "\n" + m_score.ScoreText;
        }
        else if(m_gameManager3.StateGet() == 3)
        {
            End("Game  Ovar\nScore  " + m_score.ScoreGet);

        }
        else if(m_gameManager3.StateGet() == 4)
        {
            End("Game  Clear");
        }
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
