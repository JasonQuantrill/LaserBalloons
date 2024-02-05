using UnityEngine;

public class BalloonBehaviour : MonoBehaviour
{
    public float disappearHeight = 10f;

    void Update()
    {
        if (transform.position.y >= disappearHeight)
        {
            Destroy(gameObject);
        }
    }
}
