using UnityEngine;

public class BloodScript : MonoBehaviour
{
    public ParticleSystem blood;
    
    private ParticleSystem particleInstance;
    
    public void SpawnBlood(Transform source, Vector2 direction)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.left, direction); 
        
        particleInstance = Instantiate(blood, source.position, spawnRotation);
    }
}
