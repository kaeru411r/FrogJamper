using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// life回復アイテムコンポーネント
/// </summary>

public class AddLife : ItemBase
{
    public override void Use()
    {
        GameObject.Find("Frog").GetComponent<FrogController>().AddLife();   //  プレイヤーのAddLife関数を呼ぶ
        Destroy();
    }
}
