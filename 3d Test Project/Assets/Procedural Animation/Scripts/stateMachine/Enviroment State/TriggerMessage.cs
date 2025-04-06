using UnityEngine;

public class TriggerMessage : MonoBehaviour
{
    [SerializeField] private string messageToDisplay = "You have triggered the zone!";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[TriggerMessage] {messageToDisplay} Triggered by: {other.gameObject.name}");
    }
}
