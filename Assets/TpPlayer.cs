using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpPlayer : MonoBehaviour
{
    public GameObject objectToTeleport; // Obiekt, który chcemy teleportowaæ
    public GameObject targetObject; // Obiekt, do którego chcemy siê teleportowaæ
    public float teleportHeightOffset = 2f; // Przesuniêcie w górê

    void Awake()
    {
        if (PlayerPrefs.HasKey("posX") && PlayerPrefs.HasKey("posY") && PlayerPrefs.HasKey("posZ")) 
        {
            return;
        }
        if (objectToTeleport != null && targetObject != null)
        {
            // Pobierz pozycjê obiektu docelowego
            Vector3 targetPosition = targetObject.transform.position;

            // Dodaj przesuniêcie w górê
            targetPosition.y += teleportHeightOffset;

            // Ustaw pozycjê i rotacjê obiektu do teleportacji na pozycjê i rotacjê obiektu docelowego
            objectToTeleport.transform.SetPositionAndRotation(targetPosition, targetObject.transform.rotation);
        }
    }
}
