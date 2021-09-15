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
    ///<summary>ライフのImageコンポーネント</summary>
    List<Image> m_images = new List<Image>();
    [Tooltip("各ライフのマテリアル")]
    [SerializeField] Material[] m_material = default;

    /// <summary>life表示用オブジェクトの数</summary>
    int m_range = 0;


    private void Start()
    {

        for (int i = 0; i < m_lifes.Count; i++)
        {
            m_images.Add(m_lifes[i].GetComponent<Image>());
        }

        m_range = m_lifes.Count;

        LifeUpdate();
    }

    /// <summary>lifeの表示を更新</summary>
    public void LifeUpdate()
    {
        //  lifeの値
        int life = m_frogController.LIfe;
        int index = 0;

        //  indexが万が一にもはみ出さないように想定より大きな数値のときは余剰分を切る
        if(life > (m_material.Length - 1) * m_range)
        {
            life = (m_material.Length - 1) * m_range;
        }
        //  lifeが0未満なら0に
        else if(life < 0)
        {
            life = 0;
        }

        //  lifeに使用する色を決定
        for (int i = 0; i <= life / m_range; i++)
        {
            index++;
        }

        //  lifeの左側(大きい方)を変更
        for (int i = 0; i < life % m_range; i++)
        {
            if(index >=4)
            Debug.LogError(life + "" + m_range);
            m_images[i].material = m_material[index];
        }
        //  lifeの右側(小さい方)を変更
        for (int i = 0; i <= (m_range - 1) - life % m_range; i++)
        {
            m_images[(m_range-1) - i].material = m_material[index - 1];
        }
    }

    public void End()
    {

    }
}
