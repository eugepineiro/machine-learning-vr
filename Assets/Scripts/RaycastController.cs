using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private AudioSource _activeAudioSource;

    void Start()
    {
  
        
    }
    void Update()
    {
        
        var didHit = Physics.Raycast(this.transform.position, this.transform.forward, out var hit, 50);
        if (didHit && hit.transform.gameObject.name.Contains("Cluster") )
        {
            Debug.Log($"HITTTTTTTTT {hit.transform.gameObject.name}");
          
            GameObject audio = GameObject.Find($"Audio{hit.transform.gameObject.name}");
           
            AudioSource _audioSource = audio.GetComponent<AudioSource>();
            if (_audioSource != _activeAudioSource) _activeAudioSource = _audioSource;
            
            if (!_activeAudioSource.isPlaying) _activeAudioSource.Play();
        }
        else
        {
            if (_activeAudioSource != null) _activeAudioSource.Stop();
        }
        
        Debug.DrawRay(this.transform.position, this.transform.forward * 50, Color.green,15);
    }

}