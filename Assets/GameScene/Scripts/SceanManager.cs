using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManager : MonoBehaviour
{
    [SerializeField] GameManager3 m_gameManager = default;

    [SerializeField] Score m_score = default;

    [SerializeField] Scene m_titleScene;

    string m_thisSceanName;

    [Tooltip("タイトルシーン")]
    [SerializeField] string m_titleSceanName;

    private void Start()
    {
        //DontDestroyOnLoad(this);

        PlayerPrefs.Save();
        m_thisSceanName = SceneManager.GetActiveScene().name;
    }
    public void ReStart(string nowScean)
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(nowScean);
    }
    /// <summary>
    ///     ゲームリプレイ時に呼ばれる
    ///     スコアのカウントが停止
    ///     スコア以外のゲーム中のオブジェクトを消す   
    /// </summary>
    public void GameReplay()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(m_thisSceanName);
    }

    /// <summary>
    /// ゲームを終了
    /// </summary>
    public void Exit()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(m_titleScene.handle);
        Destroy(gameObject, 0);
    }

    public void ReStart()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(m_thisSceanName);
        //m_gameManager.SetUp();
    }
}
