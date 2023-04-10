using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lossLife : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void lifeAudio()
    {
        GetComponent<AudioSource>().Play();
    }
}
