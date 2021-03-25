﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Popup_Pazuru_Managear : Popup
{  
    [SerializeField]
    private GameObject _Crear_Popup;
    [SerializeField]
    private GameObject _GameOver_Popup;

    private void Start()
    {
        
        


    }

    public void ClearPopup() 
    {
        _Crear_Popup.SetActive(true);
        PopupStart(_Crear_Popup);
    }

    public void GameOver() 
    {
        _GameOver_Popup.SetActive(true);
        Popup_Close(_GameOver_Popup);
    
    }

    public void Retry() 
    {

        SceneManager.LoadScene("");
    
    
    }



}