using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class spawn_ctrl : MonoBehaviour
{
    [SerializeField] private GameObject eCube = null;
    [SerializeField] private GameObject eSphere = null;
    [SerializeField] private GameObject eCapsule = null;
    [SerializeField] private GameObject eBoss = null;
    private int gameLevel = 0; //몇 라운드인지 확인 
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("desFire", 0, 4f); //파티클 삭제 관리 
    }
    void Update()
    {
        if (Input.GetKeyDown("space") && gameManager.instance.mGameDelay)
        {
            gameManager.instance.mGameDelay = false;
            gameLevel++;
            spawn(gameLevel);
            desPar();
        }
    }
    void spawn(int lv)
    {
        gameManager.instance.enemyDeath = 0;
 
        if (lv < 4) 
        {
            switch (lv)
            {
                case 1:
                    StartCoroutine("stage1", 10);
                    break;
                case 2:
                    StartCoroutine("stage1", 15);
                    break;
                case 3:
                    StartCoroutine("stage1", 20);
                    break;
            }
        }
        else if (lv < 7)
        {
            switch (lv)
            {
                case 4:
                    StartCoroutine("stage2", 10);
                    break;
                case 5:
                    StartCoroutine("stage2", 15);
                    break;
                case 6:
                    StartCoroutine("stage2", 20);
                    break;
            }
        }
        else if (lv <= 10)
        {
            switch (lv)
            {
                case 7:
                    StartCoroutine("stage3", 10);
                    break;
                case 8:
                    StartCoroutine("stage3", 15);
                    break;
                case 9:
                    StartCoroutine("stage3", 25);
                    break;
                case 10:
                    StartCoroutine("stageBoss", 30);
                    break;
            }
        }
    }
    IEnumerator stage1(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(eCube, this.transform.position, this.transform.rotation);
            yield return new WaitForSeconds(1.0f);  
        }
    }

    IEnumerator stage2(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(eSphere, this.transform.position, this.transform.rotation);
            yield return new WaitForSeconds(0.7f);
        }
    }

    IEnumerator stage3(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(eCapsule, this.transform.position, this.transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator stageBoss(int num)
    {
        for(int i= 0; i < num; i++)
        {
            Instantiate(eBoss, this.transform.position, this.transform.rotation);
            yield return new WaitForSeconds(0.3f);
        }
        
    }
    private void desPar()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("particle");
        for (int i = 0; i < a.Length; i++)
        {
            Destroy(a[i]);
        }
    }
    private void desFire() //fire, explosion 파티클 삭제 
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("fire");
        if (a != null)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Destroy(a[i]);
            }
        }
        a = GameObject.FindGameObjectsWithTag("Ex");
        if (a != null)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Destroy(a[i]);
            }
        }
    }
}
