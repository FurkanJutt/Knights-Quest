using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource BGsound;
    public AudioSource Fx;
    public AudioSource MonsterSound;
    public static SoundManager Instance;



    public AudioClip MonsterSouns, SwordAttack, Death;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }




    public void PlaySwrodAttack()
    {
        Fx.clip = SwordAttack;
        Fx.Play();
    }
    public void PlayDeath()
    {
        Fx.clip = Death;
        Fx.Play();
    }
    public void Playmonster()
    {
        MonsterSound.clip = MonsterSouns;
        MonsterSound.Play();
        MonsterSound.loop = true;

    }

    public void stopMonsterSound()
    {
        MonsterSound.Stop();
        MonsterSound.loop = false;
    }
}
