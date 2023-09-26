using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTranslator : MonoBehaviour
{
    private void Start()
    {
        //SceneManager.LoadSceneAsync("SaveTheVillageScene", LoadSceneMode.Additive);
    }
    public void LoadMainScene()
    {
        SceneManager.LoadSceneAsync("SaveTheVillageScene");
        //SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
    }
}
