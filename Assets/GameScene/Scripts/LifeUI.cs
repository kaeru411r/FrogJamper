using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// プレイヤーの残機を表示するコンポーネント
/// 赤丸が残機一つ分、オレンジの丸が残機二つ分
/// ライフが6以上の時に赤丸がオレンジに変わっていく
/// </summary>
public class LifeUI : MonoBehaviour
{
    [Tooltip("FrogControllerコンポーネント")]
    [SerializeField] FrogController m_frogController = default;

    [Tooltip("life表現用の丸")]
    [SerializeField] List<GameObject> m_lifes = default;
    [Tooltip("0life用マテリアル")]
    [SerializeField] Material m_nonLife = default;
    [Tooltip("1life用マテリアル")]
    [SerializeField] Material m_oneLife = default;
    [Tooltip("2life用マテリアル")]
    [SerializeField] Material m_twoLife = default;
    ///<summary>ライフのImageコンポーネント</summary>
    List<Image> m_images = new List<Image>();

    private void Start()
    {
        for(int i = 0; i < m_lifes.Count; i++)
        {
            m_images.Add(m_lifes[i].GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_frogController.LIfe <= 5)
        {
            for (int i = 0; i < m_frogController.LIfe; i++)
            {
                m_images[i].material.color = m_oneLife.color;
            }
            for (int i = 0; i < 4 - m_frogController.LIfe; i++)
            {
                m_images[4 - i].material.color = m_nonLife.color;
            }
        }
        else
        {
            for (int i = 0; i < m_frogController.LIfe; i++)
            {
                m_images[i].color = m_twoLife.color;
            }
            for (int i = 0; i < 9 - m_frogController.LIfe; i++)
            {
                m_images[4 - i].color = m_oneLife.color;
            }
        }
    }
}
