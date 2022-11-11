using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Factory Pattern (spawner:audios)
    [SerializeField] private Audio audioPrefab;
    private const int TOTAL_CLUSTERS = 5;
    private Spawner<Audio> _audioFactory = new Spawner<Audio>();
    public List<Audio> AudioInstances => _audioInstances;
    private List<Audio> _audioInstances = new List<Audio>();
    private Transform _audioParent;
    private Vector3 _audioInitialPosition = new Vector3(0, 0, 0);
    private AudioSource _audioSource;
    private AudioClip audio1;
    void Start()
    {
        fetchAudioClips();
        
        _audioParent = GameObject.Find("Audios").transform;
        
        for (int i = 0; i < TOTAL_CLUSTERS; i++)
        {
            var audio = _audioFactory.Create(audioPrefab);
            audio.transform.parent = _audioParent;
            
            // Set Audio properties
            audio.transform.position = _audioInitialPosition + new Vector3(0.5F*i,0,0.2F);
            _audioSource = audio.GetComponent<AudioSource>();
            _audioSource.clip = audio1;
            _audioSource.Play();
            
            _audioInstances.Add(audio); // Instantiate audio
        }

    }
    
    private void fetchAudioClips() {
        audio1 = Resources.Load<AudioClip>("Audio/piano-mp3/A0");
        
    }
}
