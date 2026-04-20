using UnityEngine;

public class AnimEventCaller : MonoBehaviour
{
    [SerializeField] private ComboScript combo;
    [SerializeField] private PlayerScript player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OpenWindow()
    {
        combo.ComboWindow();
    }

    public void CloseWindow()
    {
        combo.CloseComboWindow();
    }
    
    public void ComboEnd()
    {
        combo.ComboEnd();
    }
    
    public void Attack()
    {
        player.Attack();
    }
}
