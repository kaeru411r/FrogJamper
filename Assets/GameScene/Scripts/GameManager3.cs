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


    //  gameoverの参照先
    [SerializeField] GameObject frog = default;
    /// <summary>プレイヤーコントロールクラス</summary>
    [SerializeField] FrogController frogController = default;
    //

    //  メニュー関連
    /// <summary>メニュー</summary>
    [SerializeField] GameObject menu = default;
    /// <summary>メニューを開いているか</summary>
    bool openMenu = false;
    //


    /// <summary>ゲームオーバー判定</summary>
    bool gameover = false;

    /// <summary>ゲームオーバー表示</summary>
    [SerializeField] GameObject gameOverText = default;

    /// <summary>スコアボード</summary>
    [SerializeField] Score score = default;


    private void Start()
    {
        //  乱数のseed値変更
        UnityEngine.Random.InitState(DateTime.Now.Second);

        //  フレームレートの固定
        Application.targetFrameRate = 60; //60FPSに設定
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && openMenu)  //メニュー閉じる
        {
            menu.SetActive(false);
            openMenu = false;
            Time.timeScale = 1;
            frogController.Stop();
        }
        else if (Input.GetButtonDown("Cancel") && openMenu != true) //  メニュー開く
        {
            menu.SetActive(true);
            openMenu = true;
            Time.timeScale = 0;
            frogController.Stop();
        }


        //  ゲームオーバー中の処理
        if (gameover)
        {
            if (Input.anyKeyDown && Input.GetButton("Cancel") != true && openMenu != true)   //  メニューの操作を阻害しない範囲で何かしらを押してこのシーンをリセット
            {
                SceneManager.LoadScene("0_1 FrogJamper");
            }
        }


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
    }

    /// <summary>
    /// ゲームを終了
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    public void CloseMenu()
    {
        menu.SetActive(false);
        openMenu = false;
        Time.timeScale = 1;
        frogController.Stop();
    }
}

