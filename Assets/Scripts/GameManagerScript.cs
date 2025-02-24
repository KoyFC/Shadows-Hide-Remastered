using System.Collections;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance = null;

    [SerializeField] private Transform m_SpawnPoint = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void TriggerDeathSequence(Vector3 newPosition)
    {
        // Fade to black

        if (newPosition != null)
        {
            ReturnPlayerToPosition(newPosition);
        }
        else
        {
            ReturnPlayerToPosition(m_SpawnPoint.position);
        }
    }

    private IEnumerator ReturnPlayerToPosition(Vector2 newPosition, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        transform.position = newPosition;
    }
}
