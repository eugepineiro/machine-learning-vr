using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LookAtPlayerBehaviour : MonoBehaviour
{ 
    private GameObject _player;
    void Start()
    {
       	_player = GameObject.Find("XRRig");
    }
 
    void Update()
    {
        transform.LookAt(_player.transform);
    }
}