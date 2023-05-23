//This asset was uploaded by https://unityassetcollection.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomGen {

    [System.Serializable]
    public class Decoration
    {


        [SerializeField]
        public GameObject prefab;

        [SerializeField]
        public Vector3 positionOffset;

        [SerializeField]
        public Vector2 verticalRange;

        [SerializeField]
        [Range(0, 1)]
        public float probability = 0.01f;

        [SerializeField]
        [Range(0, 360)]
        public float randomRotation = 0;

        [SerializeField]
        public Vector2 scaleRange;

        [SerializeField]
        [HideInInspector]
        public bool isExpanded;


    }

}
