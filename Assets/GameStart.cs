using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("0_1 FrogJamper");
    }
}
