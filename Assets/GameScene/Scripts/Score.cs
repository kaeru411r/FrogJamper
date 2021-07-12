using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    //  テキスト本体
    [SerializeField] Text text = default;

    //  スコアを記録
    int score = 0;

    //  timeの端数を記録
    float fTime = 0;

    //  前フレームのgameover
    bool stop = false;

    //  gameoverの参照先
    GameObject frog;
    FrogController frogC;
    //


    private void Start()    //gameoverの参照先の取得
    {
        frog =  GameObject.Find("Frog");
        frogC = frog.GetComponent<FrogController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stop != true)
        {
            //  スコア計算部分
            fTime += Time.deltaTime;            //1フレームの時間fTimeを取得
            int iTime = (int)(fTime * 100);     //iTimeにfTimeを100倍した整数部分を代入
            fTime -= iTime / 100f;              //iTimeに代入した分をfTimeから引く
            score += iTime;                     //iTimeをscoreに加算
            //

            text.text = "score " + score;    //scoreを表示
        }
    }

    public void Stop()
    {
        stop = !stop;
    }

    public void Stop(bool tf)
    {
        if (tf)
        {
            stop = true;
        }
        else
        {
            stop = false;
        }
    }
}
