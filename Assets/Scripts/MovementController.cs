using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IMoveable
{
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _rotationSpeed = 20;  
    [SerializeField] private float _scaleSpeed = 5;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    public void Travel(Vector3 direction) => transform.Translate(direction * Time.deltaTime * _movementSpeed); 

    public void Rotate(Vector3 direction) => transform.Rotate(direction * Time.deltaTime * _rotationSpeed);

    public void Scale(string direction)
    {
        if (direction == "IN")
        {
            transform.localScale *= Mathf.Exp(Mathf.Log(_scaleSpeed) * Time.deltaTime);
        }
        else
        {
            transform.localScale /= Mathf.Exp(Mathf.Log(_scaleSpeed) * Time.deltaTime);
        }
    }

    public void Reset(Vector3 initialPosition, Quaternion initialRotation, Vector3 intialScale)
    {
        transform.localScale = intialScale;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
    }
    
    
}