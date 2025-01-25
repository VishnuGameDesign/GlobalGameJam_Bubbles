using GameEvents;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private BoolEventAsset _hasCollided;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out PlayerController player))
        {
            _hasCollided.Invoke(true);
        }
    }
}
