using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelTest"); //Lleva a la pantalla de juego
    }

    /* Para salir del juego desde el menu principal
    public void QuitGame()
    {
        Application.Quit();
    }
    */

    public void ConfigMenu()
    {
        SceneManager.LoadScene("ConfigMenu"); //Lleva a la configuracion
    }

    public void StoreMenu()
    {
        SceneManager.LoadScene("StoreMenu"); //Lleva a la tiendita
    }

    public void PopUp()
    {
        SceneManager.LoadScene("EmptyScene"); //En lugar de los pop-ups
    }

    public void Return()
    {
        SceneManager.LoadScene("Resolutions"); //Para regresar al menu principal
    }
}
