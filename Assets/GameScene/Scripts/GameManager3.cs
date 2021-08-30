using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


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
    [SerializeField] GameObject m_close;
    [Tooltip("タイトルに戻るボタン")]
    [SerializeField] GameObject m_exit;
    /// <summary>メニューを開いているか</summary>
    bool m_openMenu = false;
    //


    /// <summary>ゲームオーバー判定</summary>
    bool m_gameover = false;

    public bool GameState { get { return m_gameover; } }

    /// <summary>ゲーム進行状況</summary>
    static bool m_gamePlay = false;

    [Tooltip("ゲームオーバー表示")]
    [SerializeField] GameObject m_gameOverText = default;

    [Tooltip("スコアボード")]
    [SerializeField] Score m_score = default;

    [Tooltip("スコアボード背景")]
    [SerializeField] GameObject m_whiteBack = default;

    [Tooltip("タイトルシーンの名前")]
    [SerializeField] string m_titleSceanName = default;
    /// <summary>このシーンの名前</summary>
    string m_thisSceanName = default;


    //  蓮の初期生成数
    [Tooltip("蓮の初期生成数上限")]
    [SerializeField] int m_upperLimit;
    [Tooltip("蓮の初期生成数下限")]
    [SerializeField] int m_lowerLimit;
    //


    private void Start()
    {


        //  乱数のseed値変更
        UnityEngine.Random.InitState(DateTime.Now.Second);

        //  フレームレートの固定
        Application.targetFrameRate = 60; //60FPSに設定


        //ゲーム開始時の処理
        if (!m_gamePlay)
        {
            SetUp();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))  //メニュー開閉
        {
            Mnue();
        }


        //  ゲームオーバー中の処理
        //if (m_gameover)
        //{
        //    if (Input.anyKeyDown && Input.GetButton("Cancel") != true && !m_openMenu)   //  メニューの操作を阻害しない範囲で何かしらを押してこのシーンをリセット
        //    {
        //        SceneManager.LoadScene(m_thisSceanName);
        //    }
        //}


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
        m_close.SetActive(!m_openMenu);
        m_replay.SetActive(!m_openMenu);
        m_restart.SetActive(!m_openMenu);
        m_exit.SetActive(!m_openMenu);
        m_frogController.Stop();
    }

    /// <summary>
    ///     ゲームリプレイ時に呼ばれる
    ///     スコアのカウントが停止
    ///     スコア以外のゲーム中のオブジェクトを消す   
    /// </summary>
    public void GameReplay()
    {
        //  スコア固定
        m_score.Stop(true);

        //  スコア記録
        m_score.ScoreRecode();

        m_sceanManager.GameReplay();
    }

    /// <summary>
    ///   ゲームオーバー時に呼ぶ
    ///   スコアのカウントが停止
    ///   スコア以外のゲーム中のオブジェクトを消す   
    /// </summary>
    public void GameOver()
    {
        //  ゲームオーバー判定をtrueに
        m_gameover = true;

        //  ゲームプレイを停止扱いに
        m_gamePlay = true;

        //  スコア固定
        m_score.Stop(true);

        //  ゲームオーバー表示
        m_gameOverText.SetActive(true);
        m_whiteBack.SetActive(true);


        //  不必要なオブジェクトを消去
        foreach (var go in GameObject.FindGameObjectsWithTag("Gimmick"))
        {
            Destroy(go, 0);
        }
        foreach (var go in GameObject.FindGameObjectsWithTag("Lotus"))
        {
            Destroy(go, 0);
        }
        Destroy(m_frog, 0);
        //

        m_score.ScoreReset();
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

    public void SetUp()
    {
        m_gamePlay = true;
        Time.timeScale = 1;
        m_score.ScoreReset();
        m_frogController.LifeReset();
        //m_generator.SetUp();
    }
}

