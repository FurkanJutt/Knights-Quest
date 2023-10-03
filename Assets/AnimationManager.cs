using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationManager : MonoBehaviour
{
   public Transform MainMenu,Settings;


    private void Start()
    {
        MainMenu.gameObject.SetActive(true);

        MainMenu.DOScale(0.83207f, .2f).SetEase(Ease.Flash);
    }
    public void OpenMainMneu()
    {
        MainMenu.gameObject.SetActive(true);

        MainMenu.DOScale(0.83207f, .2f).SetEase(Ease.Flash);
        Settings.DOScale(0, .2f).SetEase(Ease.Flash).OnComplete(()=>Settings.gameObject.SetActive(false));
    }


    public void OpenSettings()
    {
        Settings.gameObject.SetActive(true);
        Settings.DOScale(0.83207f, .2f).SetEase(Ease.Flash);
        MainMenu.DOScale(0, .2f).SetEase(Ease.Flash).OnComplete(() => MainMenu.gameObject.SetActive(false));
    }

}
