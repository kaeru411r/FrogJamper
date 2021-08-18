using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManager : MonoBehaviour
{
    public void ReStart(string nowScean)
    {
        SceneManager.LoadScene(nowScean);
    }
}
