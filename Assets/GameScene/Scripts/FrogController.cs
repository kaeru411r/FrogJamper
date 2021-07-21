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
    [Tooltip("ジャンプ力のチャージ倍率")]
    [SerializeField] float m_accumulate = default;
    [Tooltip("ジャンプ時のスカラー量")]
    [SerializeField] float m_speed = default;
    [Tooltip("滞空時間")]
    float power = 0;
    /// <summary>チャージキャンセル</summary>
    bool cancel = false;
    [Tooltip("ジャンプ距離の円の表示レンダラー")]
    [SerializeField] LineRenderer m_lineRen = default;
    [Tooltip("円の太さ")]
    [SerializeField] float m_cirWid = default;
    /// <summary>円を構成する線の数</summary>
    int segments = 370;
    [Tooltip("円の表示をするか否か")]
    [SerializeField] bool circle = true;
    [Tooltip("円の切り替えボタン")]
    [SerializeField] Text circleButton = default;
    //

    //  旋回関係
    [Tooltip("向ける向きの限界")]
    [SerializeField] float m_agnleLimit;
    [Tooltip("旋回速度")]
    [SerializeField] float m_angVelo;
    //

    //  Frogの見た目関連
    [Tooltip("Frogの見た目")]
    [SerializeField] Sprite[] m_sprite;
    [Tooltip("水没状態の時間(秒数)")]
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

    [Tooltip("ライフ\n0になるとゲームオーバー")]
    [SerializeField] int m_life = 0;
    //  static版
    static int life = 50;

    [Tooltip("ゲームマネージャー")]
    [SerializeField] GameManager3 gm = default;

    [Tooltip("フィールド管理")]
    [SerializeField] FieldManager fieldManager = default;

    [Tooltip("スコアボード")]
    [SerializeField] Score score = default;


    /// <summary>今乗っているオブジェクト</summary>
    GameObject getOn = default;

    /// <summary>今フレームの推定fps</summary>
    float fps = 0;



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

        //  lifeのリセット
        if(life == 50)
        {
            life = m_life;
        }

        //カエルの位置を調整
        //transform.position = new Vector3(fieldManager.Position.x, fieldManager.FieldEreaUY + 1, -1f);
    }

    // Update is called once per frame
    void Update()
    {
        fps = 1 / Time.deltaTime;
        // Sink();
        if (state == FrogState.InWater)
        {
            Sink();
        }


        //  プレイヤーが一定範囲を出たらゲームオーバー
        if (transform.position.y < fieldManager.FieldEreaUY - 1)
        {
            state = FrogState.Dead;
            Death();
        }
        if (transform.position.y > fieldManager.FieldEreaTY + 1)
        {
            state = FrogState.Dead;
            Death();
        }
        if (transform.position.x < fieldManager.FieldEreaLX - 1)
        {
            state = FrogState.Dead;
            Death();
        }
        if (transform.position.x > fieldManager.FieldEreaRX + 1)
        {
            state = FrogState.Dead;
            Death();
        }
        //

        //  蓮に乗っているときに行う処理
        if (state == FrogState.Grounded && !stop)
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
            if (Input.GetButton("Fire1") && !cancel)
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
                    var rad = Mathf.Deg2Rad * i;
                    float x = (float)(transform.position.x + Mathf.Sin(rad) * m_speed * power);
                    float y = (float)(transform.position.y + Mathf.Cos(rad) * m_speed * power);
                    points[i] = new Vector3(x, y, transform.position.y - 0.1f);
                    //
                }

                //  円描画用linerendererをon
                m_lineRen.enabled = true;

                //  円の位置を設定   
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


            if (state == FrogState.MidAir)
            {
                StartCoroutine(Jump());
            }

        }
        ///  蓮に乗っているときに行う処理

        
        //power秒間飛ぶ
        //power秒間経つと処理が開始される。
        IEnumerator Jump ()
        {
            yield return new WaitForSeconds(power); //  power秒間待つ
            //  速度を0にする
            rb.velocity = new Vector2(0, 0);

            power = 0;

            //  着地か着水か
            if (contact) // 着地できる範囲に着地可能なオブジェクトがあったら接地判定をtrueにし、向きを正す
            {
                Debug.Log("着地着水");
                sr.sprite = m_sprite[0];
                state = FrogState.Grounded;
                transform.up = new Vector2(0, 0);
            }
            else  //そうでなければ水没判定をtrueにする
            {
                StartCoroutine(Sink());
                Debug.Log("水没true");
            }
        }
    }


    /// <summary>
    /// 水没時の処理
    /// 一定フレームの間水没時スプライトを適応
    /// </summary>
    IEnumerator Sink()
    {
        Debug.Log("水没");
        score.Stop(true);
        sr.sprite = m_sprite[2];
        state = FrogState.InWater;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);

        yield return new WaitForSeconds(m_splash);

        Debug.Log("ゲームオーバー");
        state = FrogState.Dead;
        Death();
    }

    /// <summary>
    /// 外部からの水没処理
    /// </summary>
    public void SinkStart()
    {
        StartCoroutine(Sink());
    }


    /// <summary>
    /// プレイヤーがいずれかの方法で死んだときに呼ぶ
    /// lifeが1以上あればscoreを引き継いで再開、そうでなければゲームオーバー
    /// </summary>
    void Death()
    {
        if(life <= 0)
        {
            LifeReset();
            gm.GameOver();
        }
        else
        {
            Debug.Log(life);
            life--;
            gm.GameReplay();
        }
    }

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
                transform.position = new Vector3(getOn.transform.position.x + 0.1f, getOn.transform.position.y , -1);
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

    /// <summary>円の表示変更</summary>
    public void Circle(bool tf)
    {
        if (tf)
        {
            circle = true;
            circleButton.text = "Circle On";
            m_lineRen.enabled = true;
        }
        else
        {
            circle = false;
            circleButton.text = "Circle Off";
            m_lineRen.enabled = false;
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

    /// <summary>lifeを初期値に戻す</summary>
    public void LifeReset()
    {
        life = 50;
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
