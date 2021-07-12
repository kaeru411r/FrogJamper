using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// プレイヤーの動かすためのコンポーネント
/// 左クリック長押しでジャンプ力をチャージ
/// 左クリックを離してジャンプ
/// チャージ中右クリックでリセット
/// 水平入力で向きを変える
/// 水面に落ちたり範囲外に出るとゲームオーバー
/// </summary>
public class FrogController : MonoBehaviour
{
    //  FrogのRigidbody
    Rigidbody2D rb = default;

    //  ジャンプ関連
    /// <summary>ジャンプ力のチャージ速度</summary>
    [SerializeField] float m_accumulate = default;
    /// <summary>ジャンプ時のスカラー量</summary>
    [SerializeField] float m_speed = default;
    /// <summary>滞空時間</summary>
    float power = 0;
    /// <summary>チャージキャンセル</summary>
    bool cancel = false;
    /// <summary>ジャンプ距離の円の表示レンダラー</summary>
    [SerializeField] LineRenderer m_lineRen = default;
    /// <summary>円の太さ</summary>
    [SerializeField] float m_cirWid = default;
    /// <summary>円を構成する線の端の数</summary>
    int segments = 380;
    /// <summary>円の表示をするか否か</summary>
    [SerializeField] bool circle = true;
    /// <summary>円の切り替えボタン</summary>
    [SerializeField] Text circleButton = default;
    //

    //  旋回関係
    /// <summary>向ける向きの限界</summary>
    [SerializeField] float m_agnleLimit;
    /// <summary>旋回速度</summary>
    [SerializeField] float m_angVelo;
    //

    //  Frogの見た目関連
    /// <summary>Frogの見た目</summary>
    [SerializeField] Sprite[] m_sprite;
    /// <summary>水没状態の時間(秒数)</summary>
    [SerializeField] float m_splash;
    /// <summary>FrogのSpriteRenderer</summary>
    SpriteRenderer sr;
    //


    //  各種状態判定
    /// <summary>着地可否判定</summary>
    bool contact = false;
    /// <summary>ポーズ</summary>
    bool stop = false;
    /// <summary>プレイヤーのステータス</summary>
    FrogState state = new FrogState();
    //

    /// <summary>ゲームマネージャー</summary>
    [SerializeField] GameManager3 gm = default;

    /// <summary>スコアボード</summary>
    [SerializeField] Score score = default;


    /// <summary>今乗っているオブジェクト</summary>
    GameObject getOn = default;

    /// <summary>今フレームの推定fps</summary>
    float fps = 0;

    /// <summary>前フレームのfps</summary>
    float bFps = 0;

    /// <summary>前々フレームのfps</summary>
    float bBFps = 0;

    /// <summary>前々々フレームのfps</summary>
    float bBBFps = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       //  速度変更用
        sr = GetComponent<SpriteRenderer>();    //  外見変更用


        //

        //  円の設定
        m_lineRen.startWidth = m_cirWid;
        m_lineRen.endWidth = m_cirWid;
        m_lineRen.positionCount = segments;
        //


        bBFps = 1 / Time.deltaTime;
        bBBFps = 1 / Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Sink();
        if (state == FrogState.InWater)
        {
            Sink();
        }
        bFps = 1 / Time.deltaTime;     //  前フレームのfps
        fps = (bFps + bBFps + bBBFps) / 3;


        //  プレイヤーが一定範囲を出たらゲームオーバー
        if (transform.position.y < -8)
        {
            gm.GameOver();
        }
        if (transform.position.y > 9)
        {
            gm.GameOver();
        }
        if (transform.position.x < -5)
        {
            gm.GameOver();
        }
        if (transform.position.x > 5)
        {
            gm.GameOver();
        }
        //

        //  蓮に乗っているときに行う処理
        if (state == FrogState.Grounded && stop != true)
        {
            //  方向操作
            if (Input.GetAxisRaw("Horizontal") < 0 && transform.rotation.z < m_agnleLimit)  //  m_angVeloの速さで左を向く
            {
                transform.Rotate(new Vector3(0, 0, m_angVelo / fps));
            }
            if (Input.GetAxisRaw("Horizontal") > 0 && transform.rotation.z > -m_agnleLimit)  //  m_angVeloの速さで右を向く
            {
                transform.Rotate(new Vector3(0, 0, -m_angVelo / fps));
            }
            //

            //  ジャンプ時間チャージ
            if (Input.GetButton("Fire1") && cancel != true)
            {
                power += m_accumulate * Time.deltaTime;
            }

            //  円描画部分
            if (circle)
            {
                var points = new Vector3[segments];
                for (int i = 0; i < segments; i++)
                {
                    //  線の位置を設定
                    var rad = Mathf.Deg2Rad * (i * 380f / segments);
                    float x = (float)(transform.position.x + Mathf.Sin(rad) * m_speed * power);
                    float y = (float)(transform.position.y + Mathf.Cos(rad) * m_speed * power);
                    points[i] = new Vector3(x, y, 0);
                    //
                }

                //  円描画用linerendererをon
                m_lineRen.enabled = true;

                //  円を表示    
                m_lineRen.SetPositions(points);
            }

            //  チャージキャンセル
            if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
            {
                power = 0;                  //  チャージをリセット
                cancel = true;              //  チャージ不可に
                m_lineRen.enabled = false;  //  円を消す
            }
            //

            //  次のチャージを許可
            if (Input.GetButton("Fire2") != true)
            {
                cancel = false;             //  チャージを可能に
            }
            //

            //  ジャンプ
            if (Input.GetButton("Fire1") != true && power > 0 && state == FrogState.Grounded)
            {
                sr.sprite = m_sprite[1];
                state = FrogState.MidAir;
                rb.velocity = transform.up * m_speed;

                //  円を消す
                m_lineRen.enabled = false;
            }

            if(state == FrogState.MidAir)
            {
                power -= Time.deltaTime;
            }
            //
        }
        ///  蓮に乗っているときに行う処理

        //  powerが0になるまでpowerを1づつ減らす
        if (power > 0 && stop != true)
        {
            power -= Time.deltaTime;
        }
        //

        //  powerが0になったフレームの処理
        if (state == FrogState.MidAir && power <= 0)
        {
            //  速度を0にする
            rb.velocity = new Vector2(0, 0);

            //  着地か着水か
            if (contact) // 着地できる範囲に着地可能なオブジェクトがあったら接地判定をtrueにし、向きを正す
            {
                Debug.Log("着地着水");
                sr.sprite = m_sprite[0];
                state = FrogState.Grounded;
                transform.up = new Vector3(0, 0, 0);
            }
            else  //そうでなければ水没判定をtrueにする
            {
                Sink();
                Debug.Log("水没true");
            }

        }



        bBFps = bFps;
        bBBFps = bBFps;
    }


    /// <summary>
    /// 水没時の処理
    /// 一定フレームの間水没時スプライトを適応
    /// </summary>
    public void Sink()
    {
        if (state != FrogState.InWater)
        {
            Debug.Log("水没");
            score.Stop(true);
            sr.sprite = m_sprite[2];
            state = FrogState.InWater;
        }
        else if (m_splash >= 0)
        {
            m_splash -= Time.deltaTime;
        }
        else
        {
            Debug.Log("ゲームオーバー");
            gm.GameOver();
        }
    }
    //

    //  着地可否判定部分
    private void OnTriggerExit2D(Collider2D collision)
    {
        //  空中におり、離脱したオブジェクトが蓮だった時着地不可
        if (state == FrogState.MidAir && collision.tag == "Lotus")
        {
            Debug.Log("着地不可");
            contact = false;
        }
        //

        //  乗っているものが消えたら水没
        if (state == FrogState.Grounded && collision.gameObject == getOn)
        {
            Debug.Log("水没");
            state = FrogState.InWater;
        }
        //
    }
    private void OnTriggerStay2D(Collider2D collision) //  接地も水没もしておらず、接触しているオブジェクトが蓮だった時着地可
    {
        if (state == FrogState.MidAir && collision.tag == "Lotus")
        {
            Debug.Log("着地場所取得");
            contact = true;
            getOn = collision.gameObject;   //  相手オブジェクトを取得
        }
        ///  着地可否判定部分


        //  着地処理
        if (state == FrogState.Grounded) //  接地していたら
        {
            Debug.Log("接地");
            if (getOn != null)  //例外対策
            {
                Debug.Log("同期");
                //  位置と移動速度を乗っているオブジェクトと同期
                rb.velocity = getOn.GetComponent<Rigidbody2D>().velocity;
                transform.position = new Vector3(getOn.transform.position.x + 0.1f, getOn.transform.position.y, getOn.transform.position.z - 1);
                //
            }
        }
        //
    }

    /// <summary>
    /// ジャンプのチャージの可否
    /// trueで停止、falseで開始
    /// </summary>
    public void Stop()
    {
        stop = !stop;
    }

    public void Stop(bool tf)
    {
        if(tf)
        {
            stop = true;
        }
        else
        {
            stop = false;
        }
    }

    /// <summary>円の表示切替</summary>
    public void Circle()
    {
        if (circle)
        {
            circle = false;
            circleButton.text = "Circle Off";
            m_lineRen.enabled = false;
        }
        else
        {
            circle = true;
            circleButton.text = "Circle On";
            m_lineRen.enabled = true;
        }
    }
}


/// <summary>
/// プレイヤーの状態
/// </summary>
enum FrogState
{
    /// <summary>物に乗っている</summary>
    Grounded,
    /// <summary>空中にいる</summary>
    MidAir,
    /// <summary>沈んでいる</summary>
    InWater,
    /// <summary>ゲームオーバー</summary>
    Dead,
}
