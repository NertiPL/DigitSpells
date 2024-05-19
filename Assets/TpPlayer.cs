using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpPlayer : MonoBehaviour
{
    public GameObject objectToTeleport; // Obiekt, kt�ry chcemy teleportowa�
    public GameObject targetObject; // Obiekt, do kt�rego chcemy si� teleportowa�
    public float teleportHeightOffset = 2f; // Przesuni�cie w g�r�

    void Awake()
    {
        if (PlayerPrefs.HasKey("posX") && PlayerPrefs.HasKey("posY") && PlayerPrefs.HasKey("posZ")) 
        {
            return;
        }
        if (objectToTeleport != null && targetObject != null)
        {
            // Pobierz pozycj� obiektu docelowego
            Vector3 targetPosition = targetObject.transform.position;

            // Dodaj przesuni�cie w g�r�
            targetPosition.y += teleportHeightOffset;

            // Ustaw pozycj� i rotacj� obiektu do teleportacji na pozycj� i rotacj� obiektu docelowego
            objectToTeleport.transform.SetPositionAndRotation(targetPosition, targetObject.transform.rotation);
        }
    }
}
