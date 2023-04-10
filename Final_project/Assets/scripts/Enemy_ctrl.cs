using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy_ctrl : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 적 속도 
    [SerializeField] private double enemyHp = 0; //적 체력 
    [SerializeField] private int mMoney = 0; //적이 죽으며 주는 돈 
    [SerializeField] private GameObject prefabHp = null; //적 HP바 
    [SerializeField] private GameObject paticleSystem = null; //적이 죽으며 생기는 파티클 
    [SerializeField] private GameObject bulletParticle = null; //적이 폭탄에 맞았을 때 생기는 파티클 

    private GameObject canvasHp;
    private Transform target;
    private int wavepointIndex = 0;
    private RectTransform hpBar; //프르팹 생성하기 위해
    public static double maxHp = 0; //적의 최대 체력을 저장
    private Slider b; //value 설정용
    private upgrade_ctrl desAudio = null;
    private lossLife lifeAudio = null;

    private void Awake()
    {
        maxHp = enemyHp;
    }
    void Start()
    {
        desAudio = GameObject.Find("Map").GetComponent<upgrade_ctrl>();
        lifeAudio = GameObject.Find("floor").GetComponent<lossLife>();
        target = Way_ctrl.points[0];
        Debug.Log(":"+ target);
        canvasHp = GameObject.Find("CanvasHp");
        hpBar = Instantiate(prefabHp, canvasHp.transform).GetComponent<RectTransform>();
        b = hpBar.gameObject.GetComponent<Slider>();
        b.value = 1f;
    }

    void Update()
    {
        Vector3 hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));
        hpBar.position = hpBarPos;
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }
     
    private void GetNextWaypoint()
    {
        if (wavepointIndex >= Way_ctrl.points.Length - 1) //finish 지점 닿으면 파괴 
        {
            gameManager.instance.gameLife--;
            if(gameManager.instance.gameLife <= 0 )
            {
                SceneManager.LoadScene("GameOver");
            }
            lifeAudio.lifeAudio();
            Destroy(b.gameObject); //hp바 
            Destroy(this.gameObject); //적 
            gameManager.instance.enemyDeath++;
            Gift();
            Clear();
            return;
        }
        wavepointIndex++;
        target = Way_ctrl.points[wavepointIndex];
        transform.LookAt(target);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet") || other.CompareTag("bullet2") || other.CompareTag("bullet3")
        || other.CompareTag("bullet4") || other.CompareTag("bulletMasic") || other.CompareTag("bulletExplosion"))//스위치문으로 변경 어떤 총알이냐에 따라서 값 변경
        {
            if (other.CompareTag("bulletExplosion"))
            {
                Instantiate(bulletParticle, this.transform.position, this.transform.rotation);
            }
            Destroy(other.gameObject);// 불릿 파괴
            ifColl(other);
            b.value = (float)(enemyHp / maxHp); //hp바 
            if(enemyHp <= 0) 
            {

                desAudio.desAudio();
                Destroy(this.gameObject);
                gameManager.instance.enemyDeath++;
                Gift();
                Clear();
                Destroy(b.gameObject);
                Instantiate(paticleSystem, this.transform.position, this.transform.rotation);
                gameManager.instance.gameMoney += mMoney;

            }
           
        }
    }
    private void ifColl(Collider other) //어떤 총알인지 비교해서 체력 닳기 
    {
        if (other.CompareTag("bullet"))
            enemyHp -= 1;
        else if (other.CompareTag("bullet2"))
            enemyHp -= 2;
        else if (other.CompareTag("bullet3"))
            enemyHp -= 3;
        else if (other.CompareTag("bullet4"))
            enemyHp -= 5;
        else if (other.CompareTag("bulletMasic"))
        {
            enemyHp -= 1;
            StartCoroutine("slowEnemy");
        }
        else if (other.CompareTag("bulletExplosion"))
            enemyHp -= 10;



    }
    private void Gift() //9라운드 끝나면 골드 get
    {
        bool isFlag = true;
        if (gameManager.instance.enemyDeath == 25 && isFlag)
        {
            isFlag = false;
            gameManager.instance.gameMoney += 500;
        }
    }
    private void Clear() //보스라운드 끝나면 게임 클리어 
    {
        if(gameManager.instance.enemyDeath == 30)
        {
            SceneManager.LoadScene("GameClear");
        }

    }
    IEnumerator slowEnemy()
    {
        if (speed > 0)
        {
            speed--;
            yield return new WaitForSeconds(1.5f);
            speed++;
        }
    }
    public void fireTur() //화염으로 때렸을 때
    {
        enemyHp -= 0.5;
        b.value = (float)(enemyHp / maxHp);
        if (enemyHp <= 0)
        {

            desAudio.desAudio();
            Destroy(this.gameObject);
            gameManager.instance.enemyDeath++;
            Gift();
            Clear();
            Destroy(b.gameObject);
            Instantiate(paticleSystem, this.transform.position, this.transform.rotation);
            gameManager.instance.gameMoney += mMoney;

        }

    }
   
}
