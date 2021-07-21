using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 初めにある程度の足場を用意しておく
/// </summary>
public class SetupGenerator : MonoBehaviour
{
    //  生成オブジェクト
    [SerializeField] GameObject m_genOb;

    //  ゲームマネージャー
    [SerializeField] GameManager3 gameManager = default;

    //  m_erea m_gemObの生成範囲
    //  (X1,Y1)と(X2,Y2)を対角の頂点とする長方形
    float m_ereaX1;
    float m_ereaX2;
    float m_ereaY1;
    float m_ereaY2;
    //

    //  初期生成数の上下限
    [SerializeField] int m_upperLimit;
    [SerializeField] int m_lowerLimit;
    //

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i < Random.Range(m_lowerLimit, m_upperLimit); i++)  //Limitで指定された数だけm_genObを生成
        {
            Instantiate(m_genOb, new Vector3(Random.Range(m_ereaX1, m_ereaX2), Random.Range(m_ereaY1, m_ereaY2)), Quaternion.Euler(0, 0, 0));
        }
    }
}
