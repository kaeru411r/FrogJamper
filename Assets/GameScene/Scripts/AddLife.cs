using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLife : ItemBase
{
    [Tooltip("FrogControllerコンポーネント")]
    [SerializeField] FrogController m_frogController = default;
    public override void Use()
    {
        m_frogController.AddLife();
        Destroy();
    }
}
