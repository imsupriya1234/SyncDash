using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 10f;// speed of player
    public float laneChangeSpeed = 5f;
    public float laneDistance = 3f; // Distance between lanes
    private int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
    public bool isStart,goLeft,goRight = false;

    // --- Other player controller
    public RemotePlayer remotePlayer;

    private float sendInterval = 0.01f;
    private float timer = 0f;

    void Update()
    {
        if (isStart)
        {
            // Move forward constantly
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            Vector3 targetPosition = new Vector3((currentLane - 1) * laneDistance, transform.position.y, transform.position.z);// Calculate target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneChangeSpeed);// Move smoothly to the target lane*/

            // Pass the value to other player
            timer += Time.deltaTime;
            if (timer >= sendInterval)
            {
                SendCompressedPosition(transform.position);
                timer = 0f;
            }
        }
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            isStart = false;
            Destroy(other.gameObject);
            if (GameManager.Instance.LifeCount >= 1)
            {
                GameManager.Instance.PlayerLife[GameManager.Instance.LifeCount-1].SetActive(false);
                GameManager.Instance.LifeCount -= 1;
            }
            GameManager.Instance.PopUp.SetActive(true);
            GameManager.Instance.DestroyBannerAd();
            GameManager.Instance.MyCoins += GameManager.Instance.AddCoin;
            GameManager.Instance.CoinCount.transform.GetChild(0).gameObject.GetComponent<Text>().text = GameManager.Instance.MyCoins.ToString();
        }
    }

    public void GoLeft()
    {
        if (currentLane > 0)
            currentLane--;
    }

    public void GoRight() 
    {
        if (currentLane < 2)
            currentLane++;
    }

    // Transfer data to the other player
    void SendCompressedPosition(Vector3 position)
    {

        float x = (float)(position.x * 100);
        float y = (float)(position.y * 100);
        float z = (float)(position.z * 100);

        Debug.Log($"[SENT] Position: ({position.x:F2}, {position.y:F2}, {position.z:F2}) | Data size: {sizeof(float) * 3 * 8} bits");


        remotePlayer.ReceiveCompressedPosition(x, y, z);
    }
}
