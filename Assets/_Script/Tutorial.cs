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
    private bool queuedTutorial;
    private int step = 0; // 0 = move, 1 = jump, 2 = attack

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
        "Attack 4 times",
        "Hold F to Block"
    };

    private readonly string[] extraMesages =
    {
        "You have an energy capacity as shown on the bottom left of your screen,",
        "You can loose energy by attacking or blocking an enemy's attack",
        "With that in mind, don't get ganged up and have fun lol"
    };

    void Start()
    {
        textMesh = text.GetComponent<TextMeshProUGUI>();
        firstEnemy.enabled = false;
        PromptStep();
    }

    void PromptStep()
    {
        if (step <= 4)
        {
            textMesh.text = extraMesages[0];
        }
        else
        {
            queuedTutorial = true;
            textMesh.text = promptMessages[step];
            text.SetActive(true);
        }

        cam.zoomIn = true;
        cam.zoomVal = 3.0f;
    }

    void Update()
    {
        if (queuedTutorial && next.action.WasPressedThisFrame())
        {
            queuedTutorial = false;
            cam.zoomIn = false;
            textMesh.text = instructionMessages[step];
            return;
        }
        else if (step <= 4  && next.action.WasPressedThisFrame())
        {
            textMesh.text = extraMesages[+1];
        }

        if (queuedTutorial) return;

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