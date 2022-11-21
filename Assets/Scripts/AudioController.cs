using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using KMeans;
namespace AudioManager
{

public class AudioController : MonoBehaviour
{
   
    private const int TOTAL_AUDIOS = 89;

    private List<List<float>> _centroids;
    private List<AudioClip> _audioClips;
    
    // Factory Pattern (spawner:audios)
    [SerializeField] private Audio _audioPrefab;
    private Spawner<Audio> _audioFactory = new Spawner<Audio>();
    public  List<Audio> AudioInstances => _audioInstances;
    private  List<Audio> _audioInstances = new List<Audio>(); 
    private Vector3 _audioInitialPosition = new Vector3(0, 0, 0);
    private AudioSource _audioSource;
    private AudioClip audio1;
    [SerializeField] public Audio audioPrefab => _audioPrefab;
 
    public void LoadAudios(int kClusters, List<DataVec> _centroids)
    {
        _audioClips = getAudioClips(kClusters);
        
        for (int i = 0; i < kClusters; i++)
        {
            var audio = _audioFactory.Create(_audioPrefab);
            var centroid = _centroids[i];
            string clusterName = $"Cluster{i+1}";

            // Set Audio properties
            audio.transform.parent = GameObject.Find(clusterName).transform;
            audio.transform.localPosition = new Vector3((float)centroid.Components[0],(float)centroid.Components[1],(float) centroid.Components[2]);
            audio.name = $"Audio{clusterName}";
            
            _audioSource = audio.GetComponent<AudioSource>();
            _audioSource.clip = _audioClips[i];
            //_audioSource.Play();
            
            _audioInstances.Add(audio); // Instantiate audio
        }

    }

    private static List<AudioClip> getAudioClips(int kClusters)
    {
        int audios_amount = (int) TOTAL_AUDIOS / kClusters;


        string[] paths = Directory.GetFiles("Assets/Resources/Audio/piano-mp3/", "*.mp3", SearchOption.AllDirectories); 

        List<AudioClip> audioClips = new List<AudioClip>();
        for (int i = 1; i <= kClusters; i++)
        {
          
            string path = Path.GetFileNameWithoutExtension(paths[i * audios_amount]);
            
            audioClips.Add(Resources.Load<AudioClip>( "Audio/piano-mp3/" + path)); 
        }

        return audioClips;

    }
    
    private static List<List<float>> getCentroids()
    {

        List<List<float>> centroids = new List<List<float>>();
 
        centroids.Add(new List<float>() {-3.5F, 2, 6.5F}); //TODO dont hardcode
        centroids.Add(new List<float>() {-2.7F, 4, -7F});
        centroids.Add(new List<float>() {4.3F, 1.7F, -5.5F});
        centroids.Add(new List<float>() {6, -3, 2.7F});
        centroids.Add(new List<float>() {-2.6F, 0.4F, -7F}); 

        return centroids;

    }
}
    
}