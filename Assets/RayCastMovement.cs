using System;
using System.Collections.Generic;
using UnityEngine;

public class RayCastMovement : MonoBehaviour
{
    [SerializeField] private int number_of_rays = 25;
    [SerializeField] private float distance = 5;
    [SerializeField] private float speed = 5;
    [SerializeField] private float rotatingAngle = 5;   
    private void Update()
    {
        CastRays();
    }
    public void CastRays()
    {
        float angle = 180.0f / (number_of_rays - 1);
        //-90 até 90
        Vector3 rayDir = new Vector3(0, -90, 0);
        float speedMultiplier = 1;
        float angleToRotate = 0;
        for (int i = 0; i < number_of_rays; i++)
        {
            if (Physics.Raycast(transform.position,
                    Quaternion.Euler(rayDir)*transform.forward, out RaycastHit hit, distance))
            {
                Debug.DrawRay(transform.position,
                Quaternion.Euler(rayDir)*transform.forward*distance, Color.red);
                speedMultiplier = Movement(i, hit, out float rot);
                angleToRotate+= rot;
            }
            else
            {
                Debug.DrawRay(transform.position,
                    Quaternion.Euler(rayDir)*transform.forward*distance, Color.green);
                
            }
            rayDir.y += angle;
        }
        transform.Rotate(new Vector3(0,angleToRotate, 0), Space.Self);
        transform.Translate( transform.forward * (speed * Time.deltaTime * speedMultiplier), Space.World);
    }
    
    public float Movement(int index, RaycastHit hit, out float rotationAngle)
    {
        float meio = -1 + (1 + number_of_rays) / 2.0f;
        float speedMultiplier = 1;
        float aux = index - meio;
        rotationAngle = 0;
        switch (aux)
        {
            //acertou na esquerda, mover para a direita
            case < 0:
                rotationAngle = -rotatingAngle/(aux*hit.distance);
                break;
            //acertou na direita, mover para a esquerda
            case > 0:
                rotationAngle = -rotatingAngle/(aux*hit.distance);
                break;
            //acertou no meio, frear
            case 0:
                Debug.Log("freando");
                speedMultiplier *= hit.distance;
                break;
        }
        //Debug.Log("index menos meio = " + aux);
        return Mathf.Min(speedMultiplier, 1f);
    }
}
