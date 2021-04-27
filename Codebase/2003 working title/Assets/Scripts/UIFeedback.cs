using UnityEngine;

public class UIFeedback : MonoBehaviour
{
    [SerializeField] AudioManager SFX = null;
    private void Start()
    {
        Time.timeScale = 1;
    }
    public void ButtonPressed()
    {
        SFX.Play("UI_Button");
    }

    public void SliderSlid()
    {
        SFX.Play("Test");
    }
}
