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
    Rigidbody2D m_rb = default;

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
    /// <summary>円のオンオフ</summary>
    static bool m_circle = true;
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
    SpriteRenderer m_sr;
    //


    //  各種状態判定
    /// <summary>着地可否判定</summary>
    bool contact = false;
    /// <summary>ポーズ</summary>
    bool stop = false;
    /// <summary>プレイヤーのステータス</summary>
    FrogState state = new FrogState();
    //

    [Tooltip("ゲーム開始時のlife")]
    [SerializeField] int m_startLife = 0;
    //  static版
    static int life = 50;
    [Tooltip("lifeの最大値")]
    [SerializeField] int m_maxLife = default;
    [Tooltip("LifeUIコンポーネント")]
    [SerializeField] LifeUI m_lifeUI = default;

    public int LIfe { get { return life; } }


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
        m_rb = GetComponent<Rigidbody2D>();       //  速度変更用
        m_sr = GetComponent<SpriteRenderer>();    //  外見変更用


        //
        Circle(m_circle);

        //  円の設定
        m_lineRen.startWidth = m_cirWid;
        m_lineRen.endWidth = m_cirWid;
        m_lineRen.positionCount = segments;
        //

    }

    // Update is called once per frame
    void Update()
    {
        fps = 1 / Time.deltaTime;
        EreaCheck();
        //

        //  蓮に乗っているときに行う処理
        if (state == FrogState.Grounded && !stop)
        {
            float bearing = Input.GetAxisRaw("Horizontal");
            if (bearing != 0)
            {
                Gyration(bearing);
            }

            //  ジャンプ時間チャージ
            if (Input.GetButton("Fire1") && !cancel)
            {
                Accumulate();
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
                Jump();
            }


            if (state == FrogState.MidAir)
            {
                StartCoroutine(AirTime());
            }

        }
        ///  蓮に乗っているときに行う処理


    }
    //power秒間飛ぶ
    //power秒間経つと処理が開始される。
    IEnumerator AirTime()
    {
        yield return new WaitForSeconds(power); //  power秒間待つ
                                                //  速度を0にする
        m_rb.velocity = new Vector2(0, 0);

        power = 0;

        //  着地か着水か
        if (contact) // 着地できる範囲に着地可能なオブジェクトがあったら接地判定をtrueにし、向きを正す
        {
            Debug.Log("着地着水");
            m_sr.sprite = m_sprite[0];
            state = FrogState.Grounded;
            transform.up = new Vector2(0, 0);
        }
        else  //そうでなければ水没判定をtrueにする
        {
            StartCoroutine(Sink());
            Debug.Log("水没true");
        }
    }

    /// <summary>
    /// 跳ぶ
    /// </summary>
    void Jump()
    {
        m_sr.sprite = m_sprite[1];
        state = FrogState.MidAir;
        m_rb.velocity = transform.up * m_speed;

        //  円を消す
        m_lineRen.enabled = false;
    }

    /// <summary>
    /// プレイヤーがエリア外に出ていたらDeathを呼ぶ
    /// </summary>
    void EreaCheck()
    {

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
    }

    /// <summary>
    /// 旋回する
    /// </summary>
    /// <param name="bearing"></param>
    void Gyration(float bearing)
    {
        if (bearing < 0 && transform.rotation.z < m_agnleLimit)  //  m_angVeloの速さで左を向く
        {
            transform.Rotate(new Vector3(0, 0, m_angVelo / fps));
        }
        if (bearing > 0 && transform.rotation.z > -m_agnleLimit)  //  m_angVeloの速さで右を向く
        {
            transform.Rotate(new Vector3(0, 0, -m_angVelo / fps));
        }
    }

    /// <summary>
    /// ジャンプ時間チャージ
    /// </summary>
    void Accumulate()
    {
        power += m_accumulate * Time.deltaTime;

        //  円描画部分
        if (m_circle)
        {
            m_lineRen.startWidth = 0.1f;
            m_lineRen.endWidth = 0.1f;
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
        else
        {
            m_lineRen.startWidth = 0;
            m_lineRen.endWidth = 0;
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
        m_sr.sprite = m_sprite[2];
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
        if (life <= 0)
        {
            LifeReset();
            gm.GameOver();
        }
        else
        {
            Debug.Log(life);
            LifeReduce();
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
                m_rb.velocity = getOn.GetComponent<Rigidbody2D>().velocity;
                transform.position = new Vector3(getOn.transform.position.x + 0.1f, getOn.transform.position.y, -1);
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
        stop = tf;
    }

    /// <summary>円の表示変更</summary>
    public void Circle(bool tf)
    {
        if (tf)
        {
            circleButton.text = "Circle Off";
            m_lineRen.startWidth = 0.1f;
            m_lineRen.endWidth = 0.1f;
        }
        else
        {
            circleButton.text = "Circle On";
            m_lineRen.startWidth = 0;
            m_lineRen.endWidth = 0;
        }
        m_circle = tf;
    }

    /// <summary>円の表示切替</summary>
    public void Circle()
    {
        if (m_circle)
        {
            circleButton.text = "Circle On";
            m_lineRen.startWidth = 0;
            m_lineRen.endWidth = 0;
        }
        else
        {
            circleButton.text = "Circle Off";
            m_lineRen.startWidth = 0.1f;
            m_lineRen.endWidth = 0.1f;
        }
        m_circle = !m_circle;
    }

    /// <summary>lifeを初期値に戻す</summary>
    public void LifeReset()
    {
        life = m_startLife;
    }

    void LifeReduce()
    {
        life--;
        m_lifeUI.LifeUpdate();
    }

    public void AddLife()
    {
        life++;
        if(life > m_maxLife)
        {
            life = m_maxLife;
        }
        m_lifeUI.LifeUpdate();
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
