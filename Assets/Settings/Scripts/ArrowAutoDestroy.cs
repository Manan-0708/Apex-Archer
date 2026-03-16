using Unity.VisualScripting;
using UnityEngine;

public class ArrowAutoDestroy : MonoBehaviour
{
   [SerializeField] private float destroyDelay = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Target"))
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}
