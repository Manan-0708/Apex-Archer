using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine.Events;
#if UNITY_EDITOR || !UNITY_EDITOR
using Oculus.Voice;
#endif

public class VoiceCommandSystem : MonoBehaviour
{
    public event System.Action<string> OnCommandRecognized;
    public event System.Action<string> OnNameCaptured;

    public enum VoiceMode { Commands, NameCapture }
    public VoiceMode currentMode = VoiceMode.Commands;

    [Header("Meta Voice Setup")]
    public AppVoiceExperience appVoiceExperience;

    // Map recognized text to commands
    private Dictionary<string, System.Action> commandMap;

    private void Awake()
    {
        if (appVoiceExperience == null)
            appVoiceExperience = GetComponent<AppVoiceExperience>();
    }

    private void OnEnable()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnSpeechResult);
            appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
            appVoiceExperience.VoiceEvents.OnError.AddListener(OnError);
        }
    }

    private void OnDisable()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnSpeechResult);
            appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
            appVoiceExperience.VoiceEvents.OnError.RemoveListener(OnError);
        }
    }

    private void OnPartialTranscription(string text)
    {
        Debug.Log("Partial Speech: " + text);
    }

    private void OnError(string error, string message)
    {
        Debug.LogError($"Wit.ai Error: {error} - {message}");
    }

    private void Start()
    {
        commandMap = new Dictionary<string, System.Action>(
            System.StringComparer.OrdinalIgnoreCase)
        {
            { "start",       () => OnCommandRecognized?.Invoke("start") },
            { "leaderboard", () => OnCommandRecognized?.Invoke("leaderboard") },
            { "quit",        () => OnCommandRecognized?.Invoke("quit") },
            { "next",        () => OnCommandRecognized?.Invoke("next") },
            { "tutorial",    () => OnCommandRecognized?.Invoke("tutorial") },
            { "play game",   () => OnCommandRecognized?.Invoke("playgame") },
            { "jungle",      () => OnCommandRecognized?.Invoke("jungle") },
            { "desert",      () => OnCommandRecognized?.Invoke("desert") },
            { "stadium",     () => OnCommandRecognized?.Invoke("stadium") },
        };
    }

    public void StartListening()
    {
        if (appVoiceExperience != null)
        {
            Debug.Log("Starting Voice Listening...");
            appVoiceExperience.Activate();
        }
        else
        {
            Debug.LogWarning("AppVoiceExperience is not assigned! Cannot start listening.");
        }
    }

    public void StopListening()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.Deactivate();
        }
    }

    // Called by your speech recognition provider when text is received
    public void OnSpeechResult(string recognizedText)
    {
        Debug.Log("Speech Recognized: " + recognizedText);
        
        if (string.IsNullOrWhiteSpace(recognizedText)) return;

        if (currentMode == VoiceMode.NameCapture)
        {
            OnNameCaptured?.Invoke(recognizedText.Trim());
            return;
        }

        // Command matching
        foreach (var kvp in commandMap)
        {
            if (recognizedText.ToLower().Contains(kvp.Key))
            {
                kvp.Value.Invoke();
                return;
            }
        }
    }
}

