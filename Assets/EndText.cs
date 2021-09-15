using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndText : MonoBehaviour
{
    [Tooltip("プレイヤーコントロールクラス")]
    [SerializeField] FrogController m_frog;
    [Tooltip("スコア管理クラス")]
    [SerializeField] Score m_score;
    [Tooltip("ゲームオーバー時スコア用テキスト")]
    [SerializeField] Text m_endText;

    private void Start()
    {

        //  残機と今ターンでのスコア表示
        m_endText.text = "Life " + m_frog.LIfe + "\n" + m_score.ScoreText;
    }
}
