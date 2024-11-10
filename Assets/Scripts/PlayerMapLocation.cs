using UnityEngine;

public class PlayerMapLocation : MonoBehaviour
{
    public Vector2 PlayerMapPosition = Vector2.zero;

    void Update()
    {
        PlayerMapPosition = new Vector2(transform.position.x, transform.position.y);
    }
}
