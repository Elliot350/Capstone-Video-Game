using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// [ExecuteInEditMode]
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private RectTransform tutorialBox;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [SerializeField] private List<Vector3> positions = new();
    [SerializeField] private List<string> texts = new();

    private int currentStep;

    [ContextMenu("AddStep")]
    private void AddStep()
    {
        Debug.Log($"Adding step");
        positions.Add(tutorialBox.position);
        texts.Add(tutorialText.text);
    }

    public void PreviousStep()
    {
        if (currentStep > 0)
        {
            currentStep--;
            ShowStep(currentStep);
        }
    }

    public void NextStep()
    {
        if (currentStep < positions.Count - 1)
        {
            currentStep++;
            ShowStep(currentStep);
        }
    }

    public void ShowStep(int stepNum)
    {
        if (positions.Count <= stepNum || texts.Count <= stepNum) return;
        Debug.Log($"Showing Step {stepNum}");
        tutorialBox.position = positions[stepNum];
        tutorialText.text = texts[stepNum];
    }
}
