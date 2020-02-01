using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager m_Instance;
    public static SoundManager Instance { get { return m_Instance; } }

    [Header("Background")]
    public AudioClip m_BackgroundMusic;

    [Header("Sounds")]
    public AudioClip m_PickPartSound;
    public AudioClip m_PickPowerUpSound;
    public AudioClip m_DropPartSound;
    public AudioClip m_StageUpSound;
    public AudioClip m_LaserSound;
    public AudioClip m_RocketSound;
    public AudioClip m_DashSound;
    public AudioClip m_GameOverSound;

    [Header("AudioSources")]
    public AudioSource m_BackgroundAudioSource;
    public AudioSource m_DefaultAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_BackgroundAudioSource.clip = m_BackgroundMusic;
        m_BackgroundAudioSource.Play();
    }

    public void PlayPickPartSound(AudioSource audioSource = null)
    {
        PlaySound(audioSource, m_PickPartSound);
    }

    public void PlayPickPowerUpSound(AudioSource audioSource = null)
    {
        PlaySound(audioSource, m_PickPowerUpSound);
    }

    public void PlayDropPartSound(AudioSource audioSource = null)
    {
        PlaySound(audioSource, m_DropPartSound);
    }

    public void PlayStageUpSound(AudioSource audioSource = null)
    {
        PlaySound(audioSource, m_StageUpSound);
    }

    public void PlayLaserSound(AudioSource audioSource = null)
    {
        PlaySound(audioSource, m_LaserSound);
    }

    public void PlayRocketSound(AudioSource audioSource = null)
    {
        PlaySound(audioSource, m_RocketSound);
    }

    public void PlayDashSound(AudioSource audioSource = null)
    {
        PlaySound(audioSource, m_DashSound);
    }

    public void PlayGameOverSound(AudioSource audioSource = null)
    {
        PlaySound(audioSource, m_GameOverSound);
    }

    public void PlaySound(AudioSource audioSource, AudioClip sound)
    {
        if (audioSource == null)
        {
            audioSource = m_DefaultAudioSource;
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}
