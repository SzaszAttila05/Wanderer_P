using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    [SerializeField] private Transform Cam;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        BaseHero currentHero = UnitManager.Instance.GetSelectedHero();


        if (currentHero != null) { 
            
            Cam.transform.position = Vector3.Lerp(currentHero.transform.position, currentHero.transform.position + new Vector3(0, 0, -10f), 0.5f);
        }   
    }
}
