using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Vars
    [SerializeField] int LevelNum;
    #endregion

    private void Awake()
    {
        if (LevelNum > 0)
            PlayerPrefs.SetInt("LastPlayed", LevelNum);
    }
    public void NewGame()
    {
        //reset all data 
        SceneManager.LoadScene("Level1");
    }

    public void Continue()
    {
        //find the last level played
        if(PlayerPrefs.GetInt("LastPlayed") != 0)
        {
            string LastPlayed = "Level" + PlayerPrefs.GetInt("LastPlayed");
            SceneManager.LoadScene(LastPlayed);
        }
    }

    public void LoadByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
