using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public PlayerStateManager playerState;

    [SerializeField] private EnemyScript firstEnemy;
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference block;

    [SerializeField] private InputActionReference next;

    public Image keyBindIcons;
    [SerializeField] private GameObject text;
    [SerializeField] private CameraScript cam;

    private TextMeshProUGUI textMesh;
    [SerializeField] private bool queuedTutorial;
    [SerializeField] private int step = 0; // 0 = move, 1 = jump, 2 = block
    private int count;

    public int comboCount;

    private readonly string[] promptMessages =
    {
        "Start moving your character around. ENTER to begin",
        "Your character is also able to jump. ENTER to begin",
        "We will then try using the attack system. ENTER to begin",
        "Your character can block attacks from enemies. Enter to begin"
    };

    private readonly string[] instructionMessages =
    {
        "Use WASD to move",
        "Press space to jump",
        "Complete Combo (M1 4 times) — 0/{0}", // Meant to be overriden
        "Hold F to Block"
    };

    private readonly string[] extraMesages =
    {
        "You have an energy capacity as shown on the bottom left of your screen,",
        "You can loose energy by attacking or blocking an enemy's attack",
        "With that in mind, have fun lol"
    };

    void Start()
    {
        textMesh = text.GetComponent<TextMeshProUGUI>();
        firstEnemy.enabled = false;
        PromptStep();
    }

    void PromptStep()
    {
        // Introduces mechanics but if all steps are done, tutorial conclusion begins
        if (step >= 4)
        {
            Debug.Log("Completed Steps");
            textMesh.text = extraMesages[0];
            text.SetActive(true);
        }
        else
        {
            queuedTutorial = true;
            textMesh.text = promptMessages[step];
            text.SetActive(true);
        }

        cam.zoomToInitial = false;
        cam.zoom = true;
        cam.zoomVal = 3.0f;
    }

    void Update()
    {
        // Will start instructing player when conditions are met
        if (queuedTutorial && next.action.WasPressedThisFrame())
        {
            queuedTutorial = false;
            cam.zoom = false;
            textMesh.text = instructionMessages[step];
            return;
        }

        // The outro of the tutorial before you play game
        else if (step >= 4  && next.action.WasPressedThisFrame())
        {
            Debug.Log("Last Texts");
            count++;

            if (count <= 2)
                textMesh.text = extraMesages[count];
            else
            {
                text.SetActive(false);
                cam.zoom = false;
                firstEnemy.enabled = true;

                this.enabled = false;
            }


        }

        if (queuedTutorial) return;

        // Following condition needing to be met to advance tutorials
        switch (step)
        {
            case 0:
                if (move.action.WasPressedThisFrame())
                    AdvanceStep();
                break;

            case 1:
                if (jump.action.WasPressedThisFrame())
                    AdvanceStep();
                break;

            case 2:
                int best = playerState.combo.bestComboStep;
                textMesh.text = $"Complete Combo (M1 4 times) — {best}/{playerState.combo.maxCombo}";

                if (playerState.combo.comboStep >= playerState.combo.maxCombo)
                    AdvanceStep();
                break;
            case 3:
                if (block.action.WasPressedThisFrame())
                    AdvanceStep();
                break;
        }
    }

    void AdvanceStep()
    {
        text.SetActive(false);
        step++;
        Invoke(nameof(PromptStep), 5.0f);
    }
}