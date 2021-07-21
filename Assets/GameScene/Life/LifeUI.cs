using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤーの残機を表示するコンポーネント
/// 赤丸が残機一つ分、オレンジの丸が残機二つ分
/// ライフが6以上の時に赤丸がオレンジに変わっていく
/// </summary>
public class LifeUI: MonoBehaviour
{
    [Tooltip("FrogControllerコンポーネント")]
    [SerializeField] FrogController m_frogController = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
