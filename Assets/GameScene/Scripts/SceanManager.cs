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

        m_thisSceanName = SceneManager.GetActiveScene().name;
    }
    public void ReStart(string nowScean)
    {
        SceneManager.LoadScene(nowScean);
    }
    /// <summary>
    ///     ゲームリプレイ時に呼ばれる
    ///     スコアのカウントが停止
    ///     スコア以外のゲーム中のオブジェクトを消す   
    /// </summary>
    public void GameReplay()
    {
        SceneManager.LoadScene(m_thisSceanName);
    }

    /// <summary>
    /// ゲームを終了
    /// </summary>
    public void Exit()
    {
        SceneManager.LoadScene(m_titleScene.name);
        Destroy(gameObject, 0);
    }

    public void ReStart()
    {
        SceneManager.LoadScene(m_thisSceanName);
        //m_gameManager.SetUp();
    }
}
