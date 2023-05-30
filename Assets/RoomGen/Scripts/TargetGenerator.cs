using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoomGen
{
    [RequireComponent(typeof(PresetEditorComponent))]
    public class TargetGenerator : MonoBehaviour
    {


        public RoomPreset preset;



        public float radius = 5;
        public int objectDensity = 500;

        [Range(0, 99999)]
        public int seed;

        public bool alignToSurface;
        public LayerMask surfaceLayer;
        public bool debug;
        private float angle = 0.5f;


        List<GameObject> generated = new List<GameObject>();
        List<Vector3> points = new List<Vector3>();




        void SpiralSequence()
        {

            Random.InitState(seed);

            for (int x = 0; x < objectDensity; x++)
            {

                float r = Mathf.Sqrt((x + angle) / objectDensity);
                float theta = Mathf.PI * (1 + Mathf.Pow(5, angle)) * (x + angle);

                float xPos = r * Mathf.Cos(theta) * radius;
                float yPos = 0;
                float zPos = r * Mathf.Sin(theta) * radius;

                Vector3 pos = new Vector3(transform.position.x + xPos, transform.position.y + yPos, transform.position.z + zPos);

                if (alignToSurface)
                {
                    Ray ray = new Ray(pos + (Vector3.up * 5f), Vector3.down);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, surfaceLayer))
                    {
                        points.Add(hit.point);
                    }
                }
                else
                {
                    points.Add(pos);
                }

                
            }


            GenerateObjects();

        }


        void GenerateObjects()
        {
            foreach (Vector3 point in points)
            {
                if (preset == null)
                {
                    return;
                }
                else
                {
                    if (!debug)
                    {
                        if (preset.floorDecorations.Count == 0)
                            return;

                        Decoration decoration = Tools.RandomDecoration(preset.floorDecorations);

                        if (decoration.prefab == null)
                            continue;

                        float probability = Random.Range(0f, 1f);

                        if (probability <= decoration.probability)
                        {
                            GameObject newObj = Instantiate(decoration.prefab, point, Quaternion.identity);
                            generated.Add(newObj);
                            newObj.transform.position = point;
                            newObj.transform.parent = transform;

                            Tools.AdjustPosition(newObj, decoration.positionOffset);
                            newObj.transform.Rotate(Vector3.up * Random.Range(0, decoration.randomRotation), Space.World);
                            newObj.transform.localScale *= Random.Range(decoration.scaleRange.x, decoration.scaleRange.y);
                        }
                    }
                }
            }
        }



        void ClearObjects()
        {
            foreach (GameObject obj in generated)
            {
                DestroyImmediate(obj);
            }

            foreach(Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }

            generated.Clear();
        }



        private void OnDrawGizmos()
        {
            if (debug)
            {
                foreach (Vector3 point in points)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(point, 0.125f);
                }
            }

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, radius);
            
        }



        public void Generate()
        {
            points.Clear();
            ClearObjects();
            SpiralSequence();
        }


    }
}
