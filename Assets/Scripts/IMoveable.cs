using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable 
{
    void Travel(Vector3 direction);
    void Rotate(Vector3 direction);
    void Scale(string direction);
    void Reset();
}