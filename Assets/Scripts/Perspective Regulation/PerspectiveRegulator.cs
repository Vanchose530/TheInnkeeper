using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PerspectiveRegulator : MonoBehaviour
{
    [HideInInspector] public bool active;

    public float ZAxisOffset;

    float deltaY;

    float deltaZ;

    void Start()
    {
        active = true;

        deltaY = 0;
    }

    void Update()
    {
        if (active)
        {
            deltaY = this.transform.position.y - PerspectiveManager.GetInstance().YStartingPoint; // count deltaY

            deltaZ = PerspectiveManager.GetInstance().ZAxisSpeed * deltaY; // count deltaZ

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, deltaZ + ZAxisOffset); // set ZAxis
        }
    }
}
