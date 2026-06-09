using System.Collections;
using UnityEngine;

public class HallwayCutscene : MonoBehaviour
{
    private PlayerStateManager player;
    private Cutscene scene;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateManager>();
        scene = FindFirstObjectByType<Cutscene>();

        StartCoroutine(PlayCutscene());
    }
    
    public IEnumerator PlayCutscene()
    {
        player.input.isEnabled = false;
        player.onCutscene = true;
        scene.camera.followPlayer = true;
        
        yield return new WaitForSeconds(0.5f);

        player.SwitchState(PlayerStateType.Walk);
        player.move = Vector2.right;

        yield return new WaitForSeconds(3.5f);
        
        scene.camera.followPlayer = true;
        player.SwitchState(PlayerStateType.Idle);

        yield return new WaitForSeconds(1.5f);

        player.onCutscene = false;
        player.input.enabled = true;

    }
}
