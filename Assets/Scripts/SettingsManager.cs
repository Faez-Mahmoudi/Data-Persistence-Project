using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Toggle soundToggle;

    private void Start()
    {
        volumeSlider.value = MyManager.Instance.volume;
        soundToggle.isOn = MyManager.Instance.soundEnabled;

        // Add listeners (optional but clean approach)
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        soundToggle.onValueChanged.AddListener(OnSoundToggled);
    }

    public void OnVolumeChanged(float value)
    {
        if (MyManager.Instance == null) return;

        Debug.Log("Volume slider changed: " + value);
        MyManager.Instance.volume = value;
        AudioListener.volume = value;
    }

    public void OnSoundToggled(bool enabled)
    {
        if (MyManager.Instance == null) return;

        MyManager.Instance.soundEnabled = enabled;
        AudioListener.pause = !enabled;
    }

    public void BackToMenu()
    {
        if (MyManager.Instance != null)
        {
            // Save settings before leaving
            MyManager.Instance.SaveScore();
        }

        SceneManager.LoadScene(0);
    }
}