using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class NarratorSystem : MonoBehaviour
{
    public static NarratorSystem Instance;

    [SerializeField] private AudioSource audioSource;

    [Header("Voice Lines")]
    [SerializeField] private AudioClip welcomeClip;       // "Welcome to the Archery Challenge"
    [SerializeField] private AudioClip speakNameClip;      // "Speak your name"
    [SerializeField] private AudioClip selectMapClip;      // "Select a map"
    [SerializeField] private AudioClip chooseModeClip;     // "Choose your game mode"

    private void Awake() => Instance = this;

    public void PlayWelcome() => Play(welcomeClip);
    public void PlaySpeakName() => Play(speakNameClip);
    public void PlaySelectMap() => Play(selectMapClip);
    public void PlayChooseMode() => Play(chooseModeClip);

    private void Play(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}

