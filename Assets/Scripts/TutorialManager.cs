using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// [ExecuteInEditMode]
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private RectTransform tutorialBox;
    [SerializeField] private RectTransform tutorialArrow;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [SerializeField] private List<Vector3> boxPositions = new();
    [SerializeField] private List<string> texts = new();
    [SerializeField] private List<Vector3> arrowPositions = new();
    [SerializeField] private List<Quaternion> arrowRotations = new();

    private int currentStep;

    [ContextMenu("AddStep")]
    private void AddStep()
    {
        Debug.Log($"Adding step");
        boxPositions.Add(tutorialBox.position);
        texts.Add(tutorialText.text);
        arrowPositions.Add(tutorialArrow.position);
        arrowRotations.Add(tutorialArrow.rotation);
    }

    [ContextMenu("RemoveAllSteps")]
    private void RemoveAllSteps()
    {
        boxPositions.Clear();
        texts.Clear();
        arrowPositions.Clear();
        arrowRotations.Clear();
    }

    [ContextMenu("GoToFirstStep")]
    private void GoToFirstStep()
    {
        currentStep = 0;
        ShowStep(currentStep);
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
        Debug.Log($"Current Step: {currentStep}, number of steps: {boxPositions.Count}");
        if (currentStep < boxPositions.Count)
        {
            currentStep++;
            ShowStep(currentStep);
        }
        else
        {
            Hide();
        }
    }

    public void ShowStep(int stepNum)
    {
        // if (boxPositions.Count <= stepNum || texts.Count <= stepNum) return;
        Debug.Log($"Showing Step {stepNum}");
        tutorialBox.gameObject.SetActive(true);
        tutorialBox.position = boxPositions[stepNum];
        tutorialText.text = texts[stepNum];
        tutorialArrow.position = arrowPositions[stepNum];
        tutorialArrow.rotation = arrowRotations[stepNum];
    }

    public void Hide()
    {
        tutorialBox.gameObject.SetActive(false);
    }

    public void Show()
    {
        ShowStep(currentStep);
    }
}
