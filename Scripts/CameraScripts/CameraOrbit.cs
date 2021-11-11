using UnityEngine;

namespace RR.CameraScripts
{
    public class CameraOrbit : MonoBehaviour 
    {
        
        [SerializeField] float rotSpeed;
        [SerializeField] Transform target;
        [SerializeField] float zoomIn;
        [SerializeField] float zoomOut;
        [SerializeField] float zoomLerpSpeed = 2.0f;
        Camera cam;
        bool isZoomed = false;
        
        void Start()
        {
            cam = Camera.main;
            //targetZoom = cam.orthographicSize;
        }
        void Update()
        {
            RotateCamera();
            RightClickZoom();
        }

        void RotateCamera()
        {
            if(Input.GetKey(KeyCode.Q))
            {
                transform.RotateAround(target.transform.position, -Vector3.up, rotSpeed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.E))
            {
                transform.RotateAround(target.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
            }
        }
        void RightClickZoom()
        {
            //float scroll = Input.GetAxis("Mouse ScrollWheel");//For taking in horizontal input

            if(Input.GetMouseButtonDown(1))
            {
                isZoomed = !isZoomed;
            }
            if(isZoomed)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomIn, zoomLerpSpeed * Time.deltaTime);
            }
            else
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomOut, zoomLerpSpeed * Time.deltaTime);
            }
        }
    }
}






