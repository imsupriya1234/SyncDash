using UnityEngine;

public class RemotePlayer : MonoBehaviour 
{
    private Vector3 targetPosition;
    public float lerpSpeed = 5f;

    public Vector3 positionOffset = new Vector3(5f, 0f, 0f); //adjust the X value as per our game scene

    public void ReceiveCompressedPosition(float x, float y, float z)
    {
        float posX = x / 100f;
        float posY = y / 100f;
        float posZ = z / 100f;

        
        targetPosition = new Vector3(posX, posY, posZ) + positionOffset;

        Debug.Log($"[RECEIVED] Position: ({targetPosition.x:F2}, {targetPosition.y:F2}, {targetPosition.z:F2})");
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
    }
}
