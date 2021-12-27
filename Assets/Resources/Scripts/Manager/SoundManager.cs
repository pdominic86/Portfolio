using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    void Awake()
    {
        if (instance != this)
        {
            var objs = FindObjectsOfType<SoundManager>();
            if (objs.Length > 1)
                Destroy(gameObject);
            else
                instance = this;
        }
        DontDestroyOnLoad(gameObject);

        var prefabs = Resources.LoadAll<AudioClip>("Sound/On");
        foreach (var prefab in prefabs)
        {
            eSoundKey soundKey = 0;
            switch (prefab.name)
            {
                case "PlayerIntro":
                    soundKey = eSoundKey.PLAYER_INTRO;
                    break;
                case "PlayerJump":
                    soundKey = eSoundKey.PLAYER_JUMP;
                    break;
                case "PlayerShoot":
                    soundKey = eSoundKey.PLAYER_SHOOT;
                    break;
                case "PlayerExmove":
                    soundKey = eSoundKey.PLAYER_EXMOVE;
                    break;
                case "GoopyPhase1Intro":
                    soundKey = eSoundKey.GOOPY_PHASE1_INTRO;
                    break;
                case "GoopyPhase1Jump":
                    soundKey = eSoundKey.GOOPY_PHASE1_JUMP;
                    break;
                case "GoopyPhase1Attack":
                    soundKey = eSoundKey.GOOPY_PHASE1_ATTACK;
                    break;
                case "GoopyPhase2Intro":
                    soundKey = eSoundKey.GOOPY_PHASE2_INTRO;
                    break;
                case "GoopyPhase2Jump":
                    soundKey = eSoundKey.GOOPY_PHASE2_JUMP;
                    break;
                case "GoopyPhase2Attack":
                    soundKey = eSoundKey.GOOPY_PHASE2_ATTACK;
                    break;
                case "GoopyDeath":
                    soundKey = eSoundKey.GOOPY_DEATH;
                    break;
                case "TombstoneDrop":
                    soundKey = eSoundKey.TOMBSTONE_DROP;
                    break;
                case "TombstoneMove":
                    soundKey = eSoundKey.TOMBSTONE_MOVE;
                    break;
                case "TombstoneAttack":
                    soundKey = eSoundKey.TOMBSTONE_ATTACK;
                    break;
                case "TombstoneDeath":
                    soundKey = eSoundKey.TOMBSTONE_DEATH;
                    break;
            }

            if (!soundList.ContainsKey(soundKey))
            {
                soundList.Add(soundKey, prefab);
            }
        }
    }

    public void PlaySound(eSoundKey _key, AudioSource _source, float _volunm = 1f)
    {
        _source.PlayOneShot(soundList[_key]);
        _source.volume = _volunm;
    }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                    instance = new GameObject("SoundManager").AddComponent<SoundManager>();
            }
            return instance;
        }
    }




    // ** »ý¼ºÀÚ
    private SoundManager() { }

    // ** Field
    private static SoundManager instance;
    Dictionary<eSoundKey, AudioClip> soundList = new Dictionary<eSoundKey, AudioClip>();
}
