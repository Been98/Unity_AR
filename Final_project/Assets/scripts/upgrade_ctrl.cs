using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//상점 
public class upgrade_ctrl : MonoBehaviour
{

    [SerializeField] private GameObject tur2 = null; //터렛생성 
    [SerializeField] private GameObject tur3 = null;
    [SerializeField] private GameObject tur4 = null;
    [SerializeField] private GameObject turMasic = null;
    [SerializeField] private GameObject turFire = null;
    [SerializeField] private GameObject turEx = null;
    [SerializeField] private TextMeshProUGUI txtMoney = null; //택스트
    [SerializeField] private TextMeshProUGUI txtlife = null; //라이프 
    [SerializeField] private TextMeshProUGUI txtUp = null; //업그레이드 창 text변경을 위해서 

    private Vector3 curTurPos = Vector3.zero;
    private int curLevel = 0;
    private Turret curLv = null;
    private GameObject mmTur;

    // Start is called before the first frame update
    void Start()
    {
        curLv = GetComponent<Turret>();
        
    }

    // Update is called once per frame
    void Update()
    {
        txtMoney.text = "돈 : " + gameManager.instance.gameMoney.ToString();
        txtlife.text = "목숨 : " + (gameManager.instance.gameLife).ToString();
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        { //게임이 진행중인가 ? Dㅕ부 묻기 
            gameManager.instance.mGameDelay = true;
        }
    }
   

    public void onClickUp(GameObject curTur) // level을 가져와서 저장 
    {
        
        curLv = curTur.GetComponent<Turret>();
        curTurPos = curTur.transform.position;
        curLevel = curLv.mlevelTur;
        mmTur = curTur;
        setText(curLevel);
    }
    public void createTur() //일반 업그레이드 
    {
        if(curLevel == 1 && gameManager.instance.gameMoney >= 30)
        {
            desTur("0");
            Instantiate(tur2, curTurPos, Quaternion.Euler(0, 0, 0));
            gameManager.instance.gameMoney -= 30;
        }
        if (curLevel == 2 && gameManager.instance.gameMoney >= 50)
        {
            desTur("0");
            Instantiate(tur3, curTurPos, Quaternion.Euler(0, 0, 0));
            gameManager.instance.gameMoney -= 50;
        }
        if (curLevel == 3 && gameManager.instance.gameMoney >= 70)
        {
            desTur("0");
            Instantiate(tur4, curTurPos, Quaternion.Euler(0, 0, 0));
            gameManager.instance.gameMoney -= 70;
        }
    }
    public void createTur(string name) //특수 업그레이드 
    {
        if (curLevel == 2 && gameManager.instance.gameMoney >= 50 && name.Equals("masic"))
        {
            desTur("0");
            Instantiate(turMasic, curTurPos, Quaternion.Euler(0, 0, 0));
            gameManager.instance.gameMoney -= 50;
        }
        if (curLevel == 3 && gameManager.instance.gameMoney >= 50 && name.Equals("masic"))
        {
            desTur("0");
            Instantiate(turFire, curTurPos, Quaternion.Euler(0, 0, 0));
            gameManager.instance.gameMoney -= 50;
        }
        if( curLevel == 4 && gameManager.instance.gameMoney >= 50 && name.Equals("masic"))
        {
            desTur("0");
            Instantiate(turEx, curTurPos, Quaternion.Euler(0, 0, 0));
            gameManager.instance.gameMoney -= 50;
        }

    }

    //판매 누르면 돈 들어오기 
    public void desTur(string text)
    {
        Destroy(mmTur);
        if (text.Equals("sell"))
        {
            gameManager.instance.gameMoney += 50;
        }
    }
    //적 죽을 때 사운드 
    public void desAudio()
    {
        GetComponent<AudioSource>().Play();
    }

    //
    private void setText(int level) 
    {
        switch(level)
        {
            case 1:
                txtUp.text = "1단계는 특수진화가 없습니다.";
                break;
            case 2:
                txtUp.text = "2단계";
                break;
            case 3:
                txtUp.text = "3단계";
                break;
            case 4:
                txtUp.text = "4단계는 업그레이드가 없습니다.";
                break;
         
        }
        if(level == 5 || level == 6 || level == 7)
        {
            txtUp.text = "특수는 업그레이드가 없습니다.";
        }
    }
    
}
