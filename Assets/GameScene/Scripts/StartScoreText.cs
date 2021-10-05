using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScoreText : MonoBehaviour
{
    [Tooltip("Textコンポーネント")]
    [SerializeField] Text m_text;
    [Tooltip("スコア表示コンポーネント")]
    [SerializeField] Score m_score;
    // Start is called before the first frame update
    void Start()
    {
        m_text.text = "Target score = " + m_score.Goal;   
    }
}
