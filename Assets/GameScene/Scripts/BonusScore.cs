using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScore : ItemBase
{
    [Tooltip("Scoreコンポーネント")]
    [SerializeField] Score m_score = default;

    [Tooltip("ボーナスの倍率")]
    [SerializeField] float m_factor = default;


    public override void Use()
    {
        for (int i = 0; i < m_factor; i++)
        {
            m_score.AddScore();
        }
    }
}
