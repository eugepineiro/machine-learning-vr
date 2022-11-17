using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBoundsCalculator : MonoBehaviour
{

    void Start() {

        var box = GetComponent<BoxCollider>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider>();

            Quaternion currentRotation = this.transform.rotation;
            this.transform.rotation = Quaternion.Euler(0f,0f,0f);

            Bounds bounds = new Bounds(this.transform.position, Vector3.zero);

            foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                if (bounds.extents == Vector3.zero)
                    bounds = renderer.bounds;
                bounds.Encapsulate(renderer.bounds);
            }

            box.center = bounds.center - this.transform.position;
            box.size = bounds.size;

            this.transform.rotation = currentRotation;
        }
    }

}
