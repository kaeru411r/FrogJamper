using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void TestReference()
    {
        GameObject go1 = GameObject.Find("Main Camera");
        GameObject go2 = go1;   // go2 に go1 を代入する
        go1.name = "ABC";
        Debug.Log(go1.name);    // go1 の名前は更新したので "ABC" となる
        Debug.Log(go2.name);    // go2 の名前も "ABC" となる
    }
}
