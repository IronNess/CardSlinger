using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class nextLevel : MonoBehaviour
{
    // List of scenes to pick from must be in build settings
    private List<string> sceneNames = new List<string>();
    int sceneCount = 0;

    private void Start()
    {
        FindLevels();
    }

    private void FindLevels()
    {
        int allScenes = SceneManager.sceneCountInBuildSettings;

        for(int i=0; i < allScenes; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneName.ToLower().Contains("level"))
            {
                sceneNames.Add(sceneName);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("colliding");
        // Check if the object hit is an enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("player");
            if (sceneNames.Count != 0)
            {
                int index = Random.Range(0, sceneNames.Count);
                // need to change to load random scene
                SceneManager.LoadScene(sceneNames[index]);
            }
            else
            {
                Debug.Log("No levels");
            }
        }
    }
}
