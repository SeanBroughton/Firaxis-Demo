using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{

    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake() 
    {
        //allows the GetPosition Method to see the MouseWorld object and it's class
        instance = this;    
    }

    public static Vector3 GetPosition()
    {
        //creates a laser to show current mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue,instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}
