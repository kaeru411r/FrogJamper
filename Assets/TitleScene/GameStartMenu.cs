using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// タイトル画面の全体管理
/// </summary>
public class GameStartMenu : MonoBehaviour
{


    //  メニュー関連
    [Tooltip("メニュー")]
    [SerializeField] GameObject m_menu = default;
    [Tooltip("メニューを開いているか")]
    bool m_openMenu = false;
    //


    /// <summary>カエルの円の表示の有無</summary>
    bool m_circle = true;
    [Tooltip("円切り替えボタンのテキスト")]
    [SerializeField] Text m_circleButton = default;


    private void Start()
    {
        if (PlayerPrefs.HasKey("Circle") || PlayerPrefs.GetInt("Circle") == 0)
        {
            Circle(true);
        }
        else
        {
            Circle(false);
        }
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
            PlayerPrefs.SetInt("Circle", 0);
            m_circleButton.text = "Circle On";
        }
        else
        {
            m_circle = true;
            PlayerPrefs.SetInt("Circle", 1);
            m_circleButton.text = "Circle Off";
        }
    }
    /// <summary>円の表示切替</summary>
    public void Circle(bool value)
    {
        if (!value)
        {
            m_circle = false;
            PlayerPrefs.SetInt("Circle", 0);
            m_circleButton.text = "Circle On";
        }
        else
        {
            m_circle = true;
            PlayerPrefs.SetInt("Circle", 1);
            m_circleButton.text = "Circle Off";
        }
    }
}
