using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string trainingScene = "TrainingScene";

    //ici c'est pour changer de scene dans le jeu y suffit de rajouter une fonction similaire a goToTrainingScene pour �a

    public void goToTrainingScene()
    {
        SceneManager.LoadScene(trainingScene);
    }
}
