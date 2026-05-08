using UnityEngine;

public class BloodScript : MonoBehaviour
{
    public ParticleSystem blood;
    
    private ParticleSystem particleInstance;
    
    public void SpawnBlood(Transform source, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
    
        Instantiate(blood, source.position, rotation);
    }
}
