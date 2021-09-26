using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// タイトル画面の全体管理
/// </summary>
public class GameStart : MonoBehaviour
{


    //  メニュー関連
    [Tooltip("メニュー")]
    [SerializeField] GameObject m_menu = default;
    [Tooltip("メニューを開いているか")]
    bool m_openMenu = false;
    //

    [Tooltip("ステージ１のインデックス")]
    [SerializeField] string m_stage1Name = default;

    /// <summary>カエルの円の表示の有無</summary>
    bool m_circle = true;
    [Tooltip("円切り替えボタンのテキスト")]
    [SerializeField] Text m_circleButton = default;


    /// <summary>ゲームを開始する</summary>
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && m_openMenu)  //メニュー閉じる
        {
            m_menu.SetActive(false);
            m_openMenu = false;
        }
        else if (Input.GetButtonDown("Cancel") && !m_openMenu) //  メニュー開く
        {
            m_menu.SetActive(true);
            m_openMenu = true;
        }
    }

    /// <summary>シーンをゲームシーンに移行する</summary>
    public void NextScene()
    {
        SceneManager.LoadScene(m_stage1Name);
        StartCoroutine(SetUp());
    }

    /// <summary>
    /// ゲームシーンが読み込まれた後、セットアップを行う
    /// </summary>
    IEnumerator SetUp()
    {
        yield return null;

        GameObject.Find("Frog").GetComponent<FrogController>().Circle(m_circle);
        GameObject.Find("GameManager").GetComponent<GameManager3>().SetUp();
        Destroy(gameObject, 0);
    }

    /// <summary>ゲームを終了</summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>メニュー切り替え</summary>
    public void Menu()
    {
        if (m_openMenu)
        {
            m_menu.SetActive(false);
            m_openMenu = false;
        }
        else
        {
            m_menu.SetActive(true);
            m_openMenu = true;
        }
    }

    /// <summary>メニュー切り替え</summary>
    public void Menu(bool tf)
    {
        if (!tf)
        {
            m_menu.SetActive(false);
            m_openMenu = false;
        }
        else
        {
            m_menu.SetActive(true);
            m_openMenu = true;
        }
    }

    /// <summary>円の表示切替</summary>
    public void Circle()
    {
        if (m_circle)
        {
            m_circle = false;
            m_circleButton.text = "Circle Off";
        }
        else
        {
            m_circle = true;
            m_circleButton.text = "Circle On";
        }
    }
}
