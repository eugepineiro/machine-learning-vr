using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AudioController : MonoBehaviour
{
    private const int TOTAL_CLUSTERS = 6;
    
    private const int TOTAL_AUDIOS = 89;

    private List<List<float>> _centroids;
    private List<AudioClip> _audioClips;
    
    // Factory Pattern (spawner:audios)
    [SerializeField] private Audio audioPrefab;
  
    private Spawner<Audio> _audioFactory = new Spawner<Audio>();
    public List<Audio> AudioInstances => _audioInstances;
    private List<Audio> _audioInstances = new List<Audio>(); 
    private Vector3 _audioInitialPosition = new Vector3(0, 0, 0);
    private AudioSource _audioSource;
    private AudioClip audio1;
    void Start()
    {
        _audioClips = getAudioClips();
        _centroids = getCentroids(); 
        
        for (int i = 0; i < TOTAL_CLUSTERS; i++)
        {
            var audio = _audioFactory.Create(audioPrefab);
            var centroid = _centroids[i];
            string clusterName = $"Cluster{i+1}";

            // Set Audio properties
            audio.transform.parent = GameObject.Find(clusterName).transform;
            audio.transform.position = _audioInitialPosition + new Vector3(centroid[0],centroid[1]+12,centroid[2]);
            audio.name = $"Audio{clusterName}";
            
            _audioSource = audio.GetComponent<AudioSource>();
            _audioSource.clip = _audioClips[i];
            //_audioSource.Play();
            
            _audioInstances.Add(audio); // Instantiate audio
        }

    }

    private List<AudioClip> getAudioClips()
    {
        int audios_amount = (int) TOTAL_AUDIOS / TOTAL_CLUSTERS;


        string[] paths = Directory.GetFiles("Assets/Resources/Audio/piano-mp3/", "*.mp3", SearchOption.AllDirectories); 

        List<AudioClip> audioClips = new List<AudioClip>();
        for (int i = 1; i <= TOTAL_CLUSTERS; i++)
        {
          
            string path = Path.GetFileNameWithoutExtension(paths[i * audios_amount]);
            
            audioClips.Add(Resources.Load<AudioClip>( "Audio/piano-mp3/" + path)); 
        }

        return audioClips;

    }
    
    private List<List<float>> getCentroids()
    {

        List<List<float>> centroids = new List<List<float>>();
 
        centroids.Add(new List<float>() {-3.5F, 2, 6.5F}); //TODO dont hardcode
        centroids.Add(new List<float>() {-2.7F, 4, -7F});
        centroids.Add(new List<float>() {4.3F, 1.7F, -5.5F});
        centroids.Add(new List<float>() {6, -3, 2.7F});
        centroids.Add(new List<float>() {-2, -4, 6.4F});
        centroids.Add(new List<float>() {-2, 1, -7.4F});

        return centroids;

    }
}
