using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//싱글톤으로 변수 4개를 제어 
public class gameManager : MonoBehaviour
{
    public static gameManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
    public int gameLife = 20;
    public int gameMoney = 200;
    public int enemyDeath = 0;
    public bool mGameDelay = true;
    
}
