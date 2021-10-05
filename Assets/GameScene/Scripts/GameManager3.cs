using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;


/// <summary>
/// ゲームシーン全体の流れを管理するコンポーネント
/// 
/// </summary>
public class GameManager3 : MonoBehaviour
{
    public static GameManager3 Instance { get; private set; }

    [Tooltip("gameoverの参照先")]
    [SerializeField] GameObject m_frog = default;
    [Tooltip("プレイヤーコントロールクラス")]
    [SerializeField] FrogController m_frogController = default;
    //[Tooltip("Generatorコンポーネント")]
    //[SerializeField] Generator m_generator = default;
    [Tooltip("SceanManagerコントロールクラス")]
    [SerializeField] SceanManager m_sceanManager = default;
    //

    //  メニュー関連
    [Tooltip("リプレイボタン")]
    [SerializeField] GameObject m_replay = default;
    [Tooltip("リスタートボタン")]
    [SerializeField] GameObject m_restart = default;
    [Tooltip("サークル表示ボタン")]
    [SerializeField] GameObject m_circle;
    [Tooltip("タイトルに戻るボタン")]
    [SerializeField] GameObject m_exit;
    [Tooltip("閉じるボタン")]
    [SerializeField] GameObject m_close;
    /// <summary>メニューを開いているか</summary>
    bool m_openMenu = false;
    //


    /// <summary>ゲームオーバー判定</summary>
    bool m_gameover = false;

    public bool GameState { get { return m_gameover; } }

    /// <summary>ゲーム進行状況</summary>
    static State m_state = new State();


    [Tooltip("ゲームオーバー表示")]
    [SerializeField] Text m_endText = default;

    //  ギミック群
    [Tooltip("蓮生成コンポーネント")]
    [SerializeField] Generator m_lotusGenerator;
    [Tooltip("アイテム生成コンポーネント")]
    [SerializeField] ItemGenerator m_itemGenerator;
    [Tooltip("EndTextコントロールクラス")]
    [SerializeField] EndText m_endTextController;
    [Tooltip("ゲーム進行用オブジェクト群")]
    [SerializeField] GameObject m_gimmicks;
    [Tooltip("ゲーム進行用UI群")]
    [SerializeField] GameObject m_uIs;
    [Tooltip("初めに乗ってる蓮")]
    [SerializeField] StartPosition m_startLotus;



    [Tooltip("死亡時の待機時間")]
    [SerializeField] float m_waitTime;

    [Tooltip("スタート時に待つ時間")]
    [SerializeField] float[] m_startWaitTime;
    [Tooltip("スタート時に表示する文字列")]
    [SerializeField] string[] m_startWaitString;

    [Tooltip("スコアボード")]
    [SerializeField] Score m_score = default;

    [Tooltip("スコアボード背景")]
    [SerializeField] GameObject m_backGround = default;


    [Tooltip("タイトルシーンの名前")]
    [SerializeField] string m_titleSceanName = default;
    /// <summary>このシーンの名前</summary>
    string m_thisSceanName = default;

    bool m_startWait = false;



    private void Start()
    {


        //  乱数のseed値変更
        UnityEngine.Random.InitState(DateTime.Now.Second);


        //ゲーム開始時の処理
        if (m_state == State.Start)
        {
            SetUp();
        }
        else
        {
            PlayStart();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))  //メニュー開閉
        {
            Mnue();
        }

    }

    /// <summary>
    /// メニューの開閉
    /// </summary>
    void Mnue()
    {
        if (m_openMenu)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
        m_openMenu = !m_openMenu;
        m_circle.SetActive(m_openMenu);
        m_restart.SetActive(m_openMenu);
        m_exit.SetActive(m_openMenu);
        m_close.SetActive(m_openMenu);
        m_frogController.Stop();
    }

    /// <summary>
    ///     ゲームリプレイ時に呼ばれる
    ///     スコアのカウントが停止
    ///     スコア以外のゲーム中のオブジェクトを消す   
    /// </summary>
    public void GameReplay()
    {
        //  状態をリプレイ待機に
        m_state = State.Replay;

        m_lotusGenerator.Destroy();
        m_itemGenerator.Destroy();
        m_score.End();
        m_endTextController.Display();
        m_backGround.SetActive(true);
        m_gimmicks.SetActive(false);
        m_uIs.SetActive(false);

        Destroy(m_frog, 0);

        StartCoroutine(ReplayStandby());
    }

    /// <summary>一定時間後にリプレイ</summary>
    IEnumerator ReplayStandby()
    {
        yield return new WaitForSeconds(m_waitTime);

        m_sceanManager.GameReplay();
    }

    /// <summary>
    ///   ゲームオーバー時に呼ぶ
    ///   スコアのカウントが停止
    ///   スコア以外のゲーム中のオブジェクトを消す   
    /// </summary>
    public void GameOver()
    {

        //  状態をゲームオーバーに
        m_state = State.GameOvar;

        m_lotusGenerator.Destroy();
        m_itemGenerator.Destroy();
        m_score.End();
        m_endTextController.Display();
        m_backGround.SetActive(true);
        m_gimmicks.SetActive(false);
        m_uIs.SetActive(false);

        //  ゲームオーバー判定をtrueに
        m_gameover = true;


        Mnue();
        m_close.SetActive(false);

        Destroy(m_frog, 0);
        //

    }

    /// <summary>ゲームクリア時に呼び出す</summary>
    public void GameClear()
    {
        //  状態をクリアに
        m_state = State.Clear;

        m_lotusGenerator.Destroy();
        m_itemGenerator.Destroy();
        m_score.End();
        m_endTextController.Display();
        m_backGround.SetActive(true);
        m_gimmicks.SetActive(false);
        m_uIs.SetActive(false);



        Mnue();
        m_close.SetActive(false);

        Destroy(m_frog, 0);

    }

    /// <summary>
    /// ゲームを終了
    /// </summary>
    public void Exit()
    {
        m_sceanManager.Exit();
    }

    public void ReStart()
    {
        m_sceanManager.ReStart();
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    public void CloseMenu()
    {
        Mnue();
    }

    /// <summary>ゲーム開始時に呼ぶ</summary>
    public void SetUp()
    {
        Time.timeScale = 1;
        m_backGround.SetActive(true);
        m_state = State.Play;
        m_startWait = true;

        StartCoroutine(Starter(m_startWaitTime[0], 0));
    }

    /// <summary>SetUpの実処理部</summary>
    IEnumerator Starter(float time, int i)
    {
        if(m_startWaitString.Length >= i + 1)
        {
            m_endText.text = m_startWaitString[i];
        }

        yield return new WaitForSeconds(m_startWaitTime[i]);

        if (m_startWaitTime.Length <= i + 1)
        {
            m_score.ScoreReset();
            m_frogController.LifeReset();
            m_backGround.SetActive(false);
            m_endText.text = "";
            m_itemGenerator.SetUp();
            m_lotusGenerator.SetUp();
            m_startLotus.Timer();
            m_startWait = false;
        }
        else
        {
            i++;
            StartCoroutine(Starter(m_startWaitTime[i], i));
        }
    }

    /// <summary>二機目以降のゲーム開始時に呼ぶ</summary>
    void PlayStart()
    {
        m_state = State.Play;
        m_itemGenerator.SetUp();
        m_lotusGenerator.SetUp();
    }

    /// <summary>現在のプレイ状況をint型で返す</summary>
    /// <returns>0 = Start, 1 = Play, 2 = Replay, 3 = GameOver, 4 = Clear</returns>
    public int StateGet()
    {
        return (int)m_state;
    }


    /// <summary>プレイ状況</summary>
    enum State
    {
        /// <summary>プレイ開始前</summary>
        Start,
        /// <summary>プレイ中</summary>
        Play,
        /// <summary>リプレイ待機</summary>
        Replay,
        /// <summary>ゲームオーバー</summary>
        GameOvar,
        /// <summary>クリア</summary>
        Clear,
    }
}

