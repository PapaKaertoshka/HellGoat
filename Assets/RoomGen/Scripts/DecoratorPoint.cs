using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PointType
{
    Default, Wall, Floor, Door, Window
}


public struct DecoratorPoint
{
   

    public GameObject pointObject;
    public Vector3 point;
    public PointType pointType;

    public DecoratorPoint(GameObject pointObject_, Vector3 point_, PointType pointType_)
    {
        pointObject = pointObject_;
        point = point_;
        pointType = pointType_;
    }

    
}
