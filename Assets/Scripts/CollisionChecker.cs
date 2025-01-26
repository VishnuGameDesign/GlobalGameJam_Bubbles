using System.Collections;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private PlayerController player;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponentInParent<PlayerController>())
        {
            player = other.collider.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                PopBubble(player);
                ResetPosition(player);
                StartCoroutine(RespawnRoutine(player));
            }
        }
    }
    
    private void PopBubble(PlayerController player)
    {
        //TODO: POP VFX
        player.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    private void ResetPosition(PlayerController player)
    {
        player.canMove = false;
        player.transform.position = transform.position - new Vector3(0, player.PlayerData.RespawnYDistance, player.PlayerData.RespawnZDistance);
    }

    private IEnumerator RespawnRoutine(PlayerController player)
    {
        yield return new WaitForSeconds(player.PlayerData.RespawnTime);
        player.GetComponentInChildren<MeshRenderer>().enabled = true;
        player.canMove = true;
    }   
    
}
