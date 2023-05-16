using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Scene currentScene;
    private int nextScene;
    public GameObject loadingScreen;
    private int currentSceneIndex;
    
    private void Awake()
    {
        instance = this;

       SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        // PlayerPrefs.SetInt("adsOff", 0);

        //reset player prefs from previous session

        PlayerPrefs.SetInt("previousRandom", 4);  

        PlayerPrefs.SetInt("adCount", 0);  

        PlayerPrefs.SetInt("soundStage", 0);

        PlayerPrefs.SetInt("restartCount", 0);
    }

    void Update()
    {
        if ((currentScene != SceneManager.GetActiveScene() && (SceneManager.GetSceneAt(1).isLoaded))) //when scene is loaded, set the second scene (the game scene) in the hierarchy as the active scene
        {
            currentScene = SceneManager.GetSceneAt(1);
            SceneManager.SetActiveScene(currentScene);
        }
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();  //add scenes being loaded/unloaded to list
    

    public void LoadNextScene() //loads next scene in the build index and unloads current scene
    {
        currentSceneIndex = currentScene.buildIndex;

        nextScene = currentSceneIndex + 1; 


        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive));
        
        StartCoroutine(GetSceneLoadProgress());
    }


    public void LoadLastScene() //loads previous scene in the build index and unloads current scene
    {
        loadingScreen.gameObject.SetActive(true);

        currentSceneIndex = currentScene.buildIndex;

        nextScene = currentSceneIndex - 1;

        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive));
        
        StartCoroutine(GetSceneLoadProgress());
    }


    public void LoadLevelSelect()
    {
        loadingScreen.gameObject.SetActive(true);

        nextScene = 4;

        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive));
       
        StartCoroutine(GetSceneLoadProgress());
    }



    public void LoadSettings()
    {
        loadingScreen.gameObject.SetActive(true);

        nextScene = 3;

        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive));
       
        StartCoroutine(GetSceneLoadProgress());
    }


    public void LoadMenu()
    {
        loadingScreen.gameObject.SetActive(true);

        nextScene = 2;

        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive));
        
        StartCoroutine(GetSceneLoadProgress());
    }


    public void LoadLevel() //load specific level based on users selection in the menu
    {

        nextScene = LevelSelection.level + 4;

        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive));
        
        StartCoroutine(GetSceneLoadProgress());
    }

    
 

    public IEnumerator GetSceneLoadProgress()
    {
        for(int i=0; i<scenesLoading.Count; i++) 
        {
            while (!scenesLoading[i].isDone) //while scenes are loading do nothing
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.25f); //delay loading screen for 0.25 seconds to give scene lighting time to render

        loadingScreen.gameObject.SetActive(false); //clear loading screen
    }
   
}
