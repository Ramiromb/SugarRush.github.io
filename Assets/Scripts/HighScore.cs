using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour {

    public LineaHS[] lineas = new LineaHS[10];
    public List<Info> sarasa = new List<Info>();
    public Highscorelist highscores = new Highscorelist();


    // Use this for initialization
    void Awake () {

      

        lineas = GetComponentsInChildren<LineaHS>();

        FirstCheck();
        
        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        highscores = JsonUtility.FromJson<Highscorelist>(jsonString);
    }

    void Start()
    {
        Cursor.visible = true;
        if (SceneManager.GetActiveScene().name == "HighScores")
        {
            for (int i = 0; i < 10; i++)
            {
                lineas[i].score.text = highscores.hclist[i].score.ToString()+ "  Kg";
                lineas[i].nickname.text = highscores.hclist[i].nickname.ToString();
            }
        }
    }


    public void AddHighscore(int score, string nickname)
    {
        Info hc = new Info { score = score, nickname = nickname };
        

        string json = JsonUtility.ToJson(CheckHC(hc, highscores));
        PlayerPrefs.SetString("HighscoreTable", json);
        PlayerPrefs.Save();
    }

    public void FirstCheck()
    {
        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        
        //highscores = JsonUtility.FromJson<Highscorelist>(jsonString);

        if (jsonString == "")
        {
            sarasa = new List<Info>()
            {
                new Info { score = 10000, nickname = "Shaggy"},
                new Info { score = 9000, nickname = "Shrek"},
                new Info { score = 8000, nickname = "Elon Musk"},
                new Info { score = 7000, nickname = "Thanos"},
                new Info { score = 6000, nickname = "Bowsette"},
                new Info { score = 5000, nickname = "Big Chungus"},
                new Info { score = 4000, nickname = "Sans"},
                new Info { score = 3000, nickname = "Mark Zucc"},
                new Info { score = 2000, nickname = "Ugandan Knuckles"},
                new Info { score = 1000, nickname = "Jhonny Jhonny"},
            };

            Highscorelist savelist = new Highscorelist { hclist = sarasa };

            string json = JsonUtility.ToJson(savelist);
            PlayerPrefs.SetString("HighscoreTable", json);
            PlayerPrefs.Save();
        }
    }

    public Highscorelist CheckHC(Info entry, Highscorelist hslist)
    {
        hslist.hclist[9] = entry;

        for (int i = 0; i < hslist.hclist.Count; i++)
        {
            for (int j = i + 1; j < hslist.hclist.Count; j++)
            {
                if (hslist.hclist[j].score > hslist.hclist[i].score)
                {
                    // Swap
                    Info tmp = hslist.hclist[i];
                    hslist.hclist[i] = hslist.hclist[j];
                    hslist.hclist[j] = tmp;
                }
            }
        }
        
        return hslist;
    }

	
    public class Highscorelist
    {
        public List<Info> hclist;
    }

    [System.Serializable]
    public class Info
    {
        public int score;
        public string nickname;
    }
}



