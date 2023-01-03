using UnityEngine;
 
public class LookAtTheCamera : MonoBehaviour 
{
    private Transform camera;
    
    void Start () 
    {
        camera = Camera.main.transform;
    }
    
    void Update()
    {
        //transform.LookAt(camera);
        transform.rotation = Quaternion.LookRotation(camera.transform.forward);
    }
 
}