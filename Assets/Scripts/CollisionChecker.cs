using System.Collections;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out PlayerController player))
        {
            ResetPosition(player);
            StartCoroutine(RespawnRoutine(player));
        }
    }
    
    private void PopBubble(PlayerController player)
    {
        //TODO: POP VFX
        player.GetComponent<Renderer>().enabled = false;
    }
    private void ResetPosition(PlayerController player)
    {
        player.canMove = false;
        player.transform.position = transform.position - new Vector3(0, 0, player.PlayerData.RespawnZDistance);
    }

    private IEnumerator RespawnRoutine(PlayerController player)
    {
        yield return new WaitForSeconds(player.PlayerData.RespawnTime);
        player.GetComponent<Renderer>().enabled = true;
        player.canMove = true;
    }   
}
