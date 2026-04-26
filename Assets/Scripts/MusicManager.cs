using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    

    void Awake()
    {
        //sigleton pattern
        if (Instance != null & Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = backgroundMusic;
        audioSource.Play();

        //scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //stops music only in shop (ALTHOUGH ADD IF WE DO MENU AND END CREDITS)
        if (scene.name == "shop")
        {
            audioSource.volume = 0f;
        }
        else
        {
            audioSource.volume = 1f;
        }
    }

}
