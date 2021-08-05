using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// スコアボーナス獲得アイテムコンポーネント
/// </summary>

public class BonusScore : ItemBase
{

    [Tooltip("ボーナスの倍率")]
    [SerializeField] float m_factor = default;


    public override void Use()
    {
        Score score = GameObject.Find("ScoreBord").GetComponent<Score>();
        for (int i = 0; i < m_factor; i++)
        {
            score.AddScore();
        }
    }
}
