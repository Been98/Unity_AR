using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//어느 씬으로 이동할지 관리
public class loadScene : MonoBehaviour
{

    public void OnClickString(string btnName)
    {
        switch (btnName)
        {
            case "start":
                SceneManager.LoadScene("Main");
                break;
            case "over":
                SceneManager.LoadScene("MainLoding");
                break;
            case "clear":
                SceneManager.LoadScene("MainLoding");
                break;
        }
    }
}
