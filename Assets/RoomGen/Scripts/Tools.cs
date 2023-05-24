using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoomGen
{
    public static class Tools
    {

        // Retrieve random objects
        public static Door RandomDoor(List<Door> objects)
        {
            if (objects.Count == 0)
                return null;
            return objects[Random.Range(0, objects.Count)];
        }

        public static Floor RandomFloor(List<Floor> objects)
        {
            if (objects.Count == 0)
                return null;
            return objects[Random.Range(0, objects.Count)];
        }

        public static Wall RandomWall(List<Wall> objects)
        {
            if (objects.Count == 0)
                return null;
            return objects[Random.Range(0, objects.Count)];
        }

        public static Window RandomWindow(List<Window> objects)
        {
            if (objects.Count == 0)
                return null;
            return objects[Random.Range(0, objects.Count)];
        }

        public static Decoration RandomDecoration(List<Decoration> objects)
        {
            if (objects.Count == 0)
                return null;
            return objects[Random.Range(0, objects.Count)];
        }


        // Transform adjustments
        public static void AdjustPosition(GameObject obj, Vector3 adjustment)
        {
            Vector3 adjustedPos = obj.transform.position;
            adjustedPos += obj.transform.right * adjustment.x;
            adjustedPos += obj.transform.up * adjustment.y;
            adjustedPos += obj.transform.forward * adjustment.z;
            obj.transform.position = adjustedPos;
        }

        public static Vector3 AdjustedPosition(GameObject obj, Vector3 adjustment)
        {
            Vector3 adjustedPos = obj.transform.position;
            adjustedPos += obj.transform.right * adjustment.x;
            adjustedPos += obj.transform.up * adjustment.y;
            adjustedPos += obj.transform.forward * adjustment.z;
            return adjustedPos;
        }


        public static void AdjustRotation(GameObject obj, Vector3 adjustment)
        {
            Vector3 adjustedPos = obj.transform.rotation.eulerAngles;
            adjustedPos += obj.transform.right * adjustment.x;
            adjustedPos += obj.transform.up * adjustment.y;
            adjustedPos += obj.transform.forward * adjustment.z;
            obj.transform.rotation = Quaternion.Euler(adjustedPos);
        }



    }
}
