using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


/// <summary>
/// ゲームシーン全体の流れを管理するコンポーネント
/// 
/// </summary>
public class GameManager3 : MonoBehaviour
{

    [Tooltip("gameoverの参照先")]
    [SerializeField] GameObject frog = default;
    [Tooltip("プレイヤーコントロールクラス")]
    [SerializeField] FrogController frogController = default;
    //

    //  メニュー関連
    [Tooltip("メニュー")]
    [SerializeField] List<GameObject> m_menus = default;
    /// <summary>メニューを開いているか</summary>
    bool openMenu = false;
    //


    /// <summary>ゲームオーバー判定</summary>
    bool gameover = false;

    [Tooltip("ゲームオーバー表示")]
    [SerializeField] GameObject gameOverText = default;

    [Tooltip("スコアボード")]
    [SerializeField] Score score = default;

    [Tooltip("スコアボード背景")]
    [SerializeField] GameObject whiteBack = default;

    [Tooltip("タイトルシーンの名前")]
    [SerializeField] string m_titleSceanName = default;
    /// <summary>このシーンの名前</summary>
    string m_thisSceanName = default;


    private void Start()
    {
        //  乱数のseed値変更
        UnityEngine.Random.InitState(DateTime.Now.Second);

        //  フレームレートの固定
        Application.targetFrameRate = 60; //60FPSに設定

        m_thisSceanName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && openMenu)  //メニュー閉じる
        {
            foreach(var menu in m_menus)
            {
                menu.SetActive(false);
            }
            openMenu = false;
            Time.timeScale = 1;
            frogController.Stop();
        }
        else if (Input.GetButtonDown("Cancel") && !openMenu) //  メニュー開く
        {
            foreach (var menu in m_menus)
            {
                menu.SetActive(true);
            }
            openMenu = true;
            Time.timeScale = 0;
            frogController.Stop();
        }


        //  ゲームオーバー中の処理
        if (gameover)
        {
            if (Input.anyKeyDown && Input.GetButton("Cancel") != true && !openMenu)   //  メニューの操作を阻害しない範囲で何かしらを押してこのシーンをリセット
            {
                SceneManager.LoadScene(m_thisSceanName);
            }
        }


    }

    /// <summary>
    ///     ゲームリプレイ時に呼ばれる
    ///     スコアのカウントが停止
    ///     スコア以外のゲーム中のオブジェクトを消す   
    /// </summary>
    public void GameReplay()
    {
        //  ゲームオーバー判定をtrueに
        gameover = true;

        //  スコア固定
        score.Stop(true);

        //  スコア記録
        score.ScoreRecode();

        SceneManager.LoadScene(m_thisSceanName);
    }

    /// <summary>
    ///   ゲームオーバー時に呼ぶ
    ///   スコアのカウントが停止
    ///   スコア以外のゲーム中のオブジェクトを消す   
    /// </summary>
    public void GameOver()
    {
        //  ゲームオーバー判定をtrueに
        gameover = true;

        //  スコア固定
        score.Stop(true);

        //  ゲームオーバー表示
        gameOverText.SetActive(true);
        whiteBack.SetActive(true);


        //  不必要なオブジェクトを消去
        foreach (var go in GameObject.FindGameObjectsWithTag("Gimmick"))
        {
            Destroy(go, 0);
        }
        foreach (var go in GameObject.FindGameObjectsWithTag("Lotus"))
        {
            Destroy(go, 0);
        }
        Destroy(frog, 0);
        //

        score.ScoreReset();
    }

    /// <summary>
    /// ゲームを終了
    /// </summary>
    public void Exit()
    {
        SceneManager.LoadScene(m_titleSceanName);
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    public void CloseMenu()
    {
        foreach (var menu in m_menus)
        {
            menu.SetActive(false);
        }
        openMenu = false;
        Time.timeScale = 1;
        frogController.Stop();
    }

    public void SetUp()
    {
        Time.timeScale = 1;
        score.ScoreReset();
        frogController.LifeReset();
    }

    [SerializeField] public struct Erea
    {
        public float RX { get; }
        public float LX { get; }
        public float RY { get; }
        public float LY { get; }
    }
}

