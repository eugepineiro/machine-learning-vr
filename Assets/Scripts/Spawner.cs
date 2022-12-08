using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner<T> : IFactory<T> where T : MonoBehaviour
{
    public  T Create(T prefab)
    {
        return GameObject.Instantiate(prefab);
    }
}