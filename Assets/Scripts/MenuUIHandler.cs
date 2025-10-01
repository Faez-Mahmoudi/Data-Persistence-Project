using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
# if UNITY_EDITOR
using UnityEditor;
# endif

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField nameInputField;
    
    public void StartNew()
    {
        if (MyManager.Instance.playerName == "")
        {
            MyManager.Instance.playerName = nameInputField.text;
        }
        else if (nameInputField.text != "" )
        {
            MyManager.Instance.playerName = nameInputField.text;
        }
  
        SceneManager.LoadScene(1); 
    }

    public void Exit()
    {
    # if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    # else
        Application.Quit();
    # endif
    }

}
