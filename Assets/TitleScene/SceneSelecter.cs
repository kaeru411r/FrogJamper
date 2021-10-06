using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelecter : MonoBehaviour
{
    public void SceneChange(int value)
    {
        SceneManager.LoadScene(value);
    }
}
