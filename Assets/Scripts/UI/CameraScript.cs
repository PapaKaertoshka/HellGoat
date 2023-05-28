using UnityEngine;
 using Vector3 = UnityEngine.Vector3;

 public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool isCustomOffest;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 0.1f;
    

    void Start()
    {
        if (!isCustomOffest)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        SmoothFollow();
    }

    private void SmoothFollow()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 smoothFollow = Vector3.Lerp(transform.position,
        targetPos, smoothSpeed);

        transform.position = smoothFollow;
        transform.LookAt(target);
    }
}
