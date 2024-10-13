using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager m;
    void Start()
    {
        m=FindObjectOfType<GameManager>();
    }

    void Update()
    {
        
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit groundHit;

            if (Physics.Raycast(cameraRay, out groundHit, Mathf.Infinity))
            {
                // احسب الاتجاه من اللاعب إلى نقطة الماوس
                Vector3 playerToMouse = groundHit.point - transform.position;
                playerToMouse.y = 0; // تأكد من أن محور Y هو صفر



                // احصل على الدوران الجديد
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                // قم بتدوير اللاعب فقط
                transform.rotation = newRotation;

            }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            StartCoroutine(FindObjectOfType<GameManager>().PlayerWin());
        }
    }

    
}
