using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceInputManager : MonoBehaviour
{
    public static VoiceInputManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject voiceInputBoard;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_Text nameDisplayText;
    [SerializeField] private TMP_Text statusText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip askNameClip;
    [SerializeField] private AudioClip confirmNameClip;
    [SerializeField] private AudioClip tryAgainClip;

    [Header("Settings")]
    [SerializeField] private float listenDuration = 8f;

    private DictationRecognizer dictationRecognizer;
    private string heardName = "";
    private bool isConfirming = false;
    private bool isListening = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartVoiceInput()
    {
        voiceInputBoard.SetActive(true);
        heardName = "";
        isConfirming = false;
        StartCoroutine(ListenForName());
    }

    private IEnumerator ListenForName()
    {
        instructionText.text = "SAY YOUR NAME";
        nameDisplayText.text = "";
        statusText.text = "Listening...";

        if (askNameClip != null)
            audioSource.PlayOneShot(askNameClip);

        // Wait for audio + small buffer
        yield return new WaitForSeconds(1.5f);

        // Start dictation as coroutine
        yield return StartCoroutine(StartDictation(false));
    }

    // ✅ Fixed — now a proper coroutine
    private IEnumerator StartDictation(bool isConfirmation)
    {
        // Dispose old recognizer cleanly
        DisposeDictation();

        yield return new WaitForSeconds(0.5f);

        dictationRecognizer = new DictationRecognizer();

        if (isConfirmation)
        {
            dictationRecognizer.DictationResult +=
                OnConfirmationResult;
        }
        else
        {
            dictationRecognizer.DictationResult +=
                OnDictationResult;
            dictationRecognizer.DictationHypothesis +=
                OnDictationHypothesis;
        }

        dictationRecognizer.DictationError +=
            OnDictationError;
        dictationRecognizer.DictationComplete +=
            OnDictationComplete;

        dictationRecognizer.Start();
        isListening = true;

        Debug.Log("Dictation started — listening...");

        // Auto stop after duration
        StartCoroutine(StopAfterDelay());
    }

    private IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(listenDuration);

        if (isListening)
        {
            Debug.Log("Listen timeout — stopping.");
            StopDictation();
        }
    }

    private void StopDictation()
    {
        isListening = false;

        if (dictationRecognizer != null &&
            dictationRecognizer.Status ==
            SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
        }
    }

    private void DisposeDictation()
    {
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationResult -=
                OnDictationResult;
            dictationRecognizer.DictationResult -=
                OnConfirmationResult;
            dictationRecognizer.DictationError -=
                OnDictationError;
            dictationRecognizer.DictationComplete -=
                OnDictationComplete;
            dictationRecognizer.DictationHypothesis -=
                OnDictationHypothesis;

            dictationRecognizer.Dispose();
            dictationRecognizer = null;
        }
    }

    private void OnDictationHypothesis(string text)
    {
        nameDisplayText.text = text;
        statusText.text = "Hearing...";
    }

    private void OnDictationResult(
        string text,
        ConfidenceLevel confidence)
    {
        Debug.Log("Heard name: " + text);

        heardName = text.Trim();

        if (heardName.Length > 0)
            heardName = char.ToUpper(heardName[0])
                + heardName.Substring(1);

        nameDisplayText.text = heardName;
        StopDictation();
        StartCoroutine(ConfirmName());
    }

    private IEnumerator ConfirmName()
    {
        isConfirming = true;

        instructionText.text = "IS THIS YOUR NAME?";
        statusText.text =
            "Say YES to confirm or NO to retry";

        if (confirmNameClip != null)
            audioSource.PlayOneShot(confirmNameClip);

        yield return new WaitForSeconds(1.5f);

        // ✅ Now uses coroutine properly
        yield return StartCoroutine(
            StartDictation(true));
    }

    private void OnConfirmationResult(
        string text,
        ConfidenceLevel confidence)
    {
        Debug.Log("Confirmation heard: " + text);

        string response = text.ToLower().Trim();
        StopDictation();

        if (response.Contains("yes"))
            ConfirmAndProceed();
        else if (response.Contains("no"))
            StartCoroutine(RetryInput());
        else
            StartCoroutine(ConfirmName());
    }

    private void ConfirmAndProceed()
    {
        PlayerManager.Instance.SetPlayer(heardName);
        voiceInputBoard.SetActive(false);
        PlayerInputManager.Instance.ShowTutorialBoard();
    }

    private IEnumerator RetryInput()
    {
        statusText.text = "Let's try again...";

        if (tryAgainClip != null)
            audioSource.PlayOneShot(tryAgainClip);

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(ListenForName());
    }

    private void OnDictationComplete(
        DictationCompletionCause cause)
    {
        isListening = false;
        Debug.Log("Dictation complete: " + cause);

        // Only retry if not confirming
        // and didn't get a result
        if (!isConfirming &&
            cause != DictationCompletionCause.Complete)
        {
            statusText.text =
                "Didn't catch that, trying again...";
            StartCoroutine(RetryInput());
        }
    }

    private void OnDictationError(
        string error, int hresult)
    {
        Debug.LogError("Dictation error: " + error);
        statusText.text = "Error — trying again...";
        StartCoroutine(RetryInput());
    }

    private void OnDestroy()
    {
        DisposeDictation();
    }
}