
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelChanger : MonoBehaviour {

    public Animator animator;

    private int levelToLoad;

    public int levelNumber;

	// Use this for initialization
	void Start () {
        Cursor.visible = true;
    }
	
	// Update is called once per frame
	void Update () {
        if(SceneManager.GetActiveScene().name == "HowToPlay")
        {
            if (Input.anyKey) FadeToLevel(1);
        }
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
        GetComponent<AudioSource>().Play();
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void Exit()
    {
        Application.Quit();
        GetComponent<AudioSource>().Play();

    }

}
