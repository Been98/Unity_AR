using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform m_tfGunBody = null;
    [SerializeField] private float m_range = 0f; //사정거리 
    [SerializeField] private LayerMask m_layerMask = 0; //특정 레이어만 공격
    [SerializeField] private float m_spinSpeed = 0f; //적발견시 포신회전
    [SerializeField] private float m_fireRate = 0f;
    [SerializeField] private int turStatus = 0;
    [SerializeField] private ParticleSystem firePar = null;

    public int mlevelTur = 0; //curTurLevel 알려주기위해 
    float m_currentFireRate;

    private upgrade_ctrl sendLevel =null;

    [SerializeField] private Transform bullet = null;

    Transform m_tfTarget = null;// 공격대상 transform

    void SearchEnemy() //적 찾기 
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_range, m_layerMask);//적들의 위치 저장
        Transform t_shortestTarget = null; //가장 짧은 적의 위치저장
        if (t_cols.Length > 0)
        {
            float t_shortestDistance = Mathf.Infinity;//가장 짧은것을 찾기위해 가장 긴것 넣기
            foreach (Collider t_colTarget in t_cols)
            {
                float t_distance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position);//제곱반환(실제거리*실제거리)
                if (t_shortestDistance > t_distance)
                {
                    t_shortestDistance = t_distance;
                    t_shortestTarget = t_colTarget.transform;
                }
            }
        }
        m_tfTarget = t_shortestTarget;//최종타켓 저장
    }
    // Start is called before the first frame update
    void Start()
    {
        m_currentFireRate = m_fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        sendLevel = GetComponent<upgrade_ctrl>();
        SearchEnemy();
        shootTur();
      
    }
    private void shootTur()
    {
        if (m_tfTarget == null)//적 찾기전 회/
            m_tfGunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
        else
        {
            Quaternion t_lookRotation = Quaternion.LookRotation(m_tfTarget.position - this.gameObject.transform.position);//LookRotation - 특정좌표를 바라보게 만드는 회전-
            Vector3 t_euler = Quaternion.RotateTowards(m_tfGunBody.rotation, t_lookRotation, m_spinSpeed * Time.deltaTime).eulerAngles;// a~b까지 c의 속도로 회/
            m_tfGunBody.rotation = Quaternion.Euler(0, t_euler.y, 0);//포신이 y축으로만 회전하기위/

            Quaternion t_fireRotation = Quaternion.Euler(0, t_lookRotation.eulerAngles.y, 0);//터렛이 조준해야 될 최종 방향
            if (Quaternion.Angle(m_tfGunBody.rotation, t_fireRotation) < 3f && turStatus == 1)
            {
                m_currentFireRate = m_fireRate;
                GameObject sp = this.transform.GetChild(1).gameObject;
                GameObject spawn_point = sp.transform.GetChild(0).gameObject;
                Instantiate(firePar, this.transform.position, this.transform.rotation);
                RaycastHit[] rayHits = Physics.SphereCastAll(this.transform.position, m_range, Vector3.forward, 0.0f, LayerMask.GetMask("Enemy"));
                foreach (RaycastHit hit in rayHits)
                {
                    hit.transform.GetComponent<Enemy_ctrl>().fireTur();
                }
            }
            else if (Quaternion.Angle(m_tfGunBody.rotation, t_fireRotation) < 3f && turStatus != 1)
            {
                m_currentFireRate -= Time.deltaTime;
                if (m_currentFireRate <= 0)
                {
                    m_currentFireRate = m_fireRate;
                    GameObject sp = this.transform.GetChild(1).gameObject;
                    GameObject spawn_point = sp.transform.GetChild(0).gameObject;
                    Transform prefab_bullet = Instantiate(bullet, spawn_point.transform.position, spawn_point.transform.rotation);
                    prefab_bullet.GetComponent<Rigidbody>().AddForce(spawn_point.transform.forward * 8000.0f);
                }
            }
        }  
    }
}
