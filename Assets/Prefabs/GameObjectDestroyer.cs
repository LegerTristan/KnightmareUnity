using UnityEngine;

public class GameObjectDestroyer : MonoBehaviour
{
    /*Lorsqu'un gameObject rentre en contact avec le Destroyer, on le détruit.*/
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.transform.parent.gameObject);
    }
}
