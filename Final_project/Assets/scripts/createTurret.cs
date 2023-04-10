using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class createTurret : MonoBehaviour
{
    [SerializeField] private GameObject turret = null; //1단계 터렛 생성
    [SerializeField] private GameObject panelUp = null; //업그레이드 패널 띄어주기 위해 

    private bool isCreat = false; //한 번 눌렀을 때 연속해서 눌리지 않도록 
    private bool isNotCreat = false;
    private GameObject tur; //터렛 생성해서 색 변경 등 사용 
    [SerializeField] private upgrade_ctrl SendTur = null; //보내줄 터렛 
    RaycastHit hit;
    Renderer[] turColor; //터렛 색 변경 


    void Update()
    {
       
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // screen -> world좌표
        int mLayerMask_tur = 1 << LayerMask.NameToLayer("Tullet");
        if (Input.GetKeyDown(KeyCode.T) && gameManager.instance.gameMoney >= 50)
        {
            isCreat = true;
            tur = Instantiate(turret, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);
            tur.transform.SetParent(this.transform, false);
            turColor = tur.gameObject.GetComponentsInChildren<Renderer>();
           
            gameManager.instance.gameMoney -= 50;
        }
        
        if (isCreat) // T를 눌렀을 때 발생
        {
            int mLayerMask = 1 << LayerMask.NameToLayer("Floor"); //생성위치 정하기
            int mLayerMask_way = 1 << LayerMask.NameToLayer("Way"); //생성 못하게 하기 위해서 
            
            
            if (Physics.Raycast(ray, out hit,Mathf.Infinity,mLayerMask))           
            {
                Vector3 v = hit.point;
                v.y = 1;
                tur.transform.position = v;
                for(int i =0; i <turColor.Length; i++)
                    turColor[i].material.color = Color.green;
            }
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, mLayerMask_way))
            {
                for (int i = 0; i < turColor.Length; i++)
                    turColor[i].material.color = Color.red;
                isNotCreat = true;
            }
            else
            {
                isNotCreat = false;
            }
            if (Input.GetMouseButtonDown(0) && !isNotCreat) // 생성하면 false로 변경하기 위해서 
            {
                isCreat = false;
                for (int i = 0; i < turColor.Length; i++)
                    turColor[i].material.color = Color.white;
            }
            
        }
        if (Input.GetMouseButtonDown(1) && Physics.Raycast(ray, out hit, Mathf.Infinity, mLayerMask_tur))
        {
            panelUp.SetActive(true);
            if (hit.collider.CompareTag("turret"))
            {
                SendTur.onClickUp(hit.collider.gameObject);
            }
        }
    }
}
