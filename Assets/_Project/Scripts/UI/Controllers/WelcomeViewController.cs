using System;
using UnityEngine;
using UnityEngine.UIElements;

public class WelcomeViewController
{
    private readonly VisualElement root;
    private readonly Action onStartExperience;

    private VisualElement howItWorksModal;

    public WelcomeViewController(VisualElement root, Action onStartExperience)
    {
        this.root = root;
        this.onStartExperience = onStartExperience;
    }

    public void Bind()
    {
        Button startExperienceButton = root.Q<Button>("start-experience-button");
        Button learnMoreButton = root.Q<Button>("learn-more-button");

        howItWorksModal = root.Q<VisualElement>("how-it-works-modal");

        Button closeHowItWorksButton = root.Q<Button>("close-how-it-works-button");
        Button modalCloseButton = root.Q<Button>("modal-close-button");
        Button modalStartButton = root.Q<Button>("modal-start-button");

        if (startExperienceButton != null)
        {
            startExperienceButton.clicked += onStartExperience;
        }
        else
        {
            Debug.LogWarning("[WelcomeViewController] No se encontró start-experience-button.");
        }

        if (learnMoreButton != null)
        {
            learnMoreButton.clicked += ShowHowItWorksModal;
        }
        else
        {
            Debug.LogWarning("[WelcomeViewController] No se encontró learn-more-button.");
        }

        if (closeHowItWorksButton != null)
        {
            closeHowItWorksButton.clicked += HideHowItWorksModal;
        }

        if (modalCloseButton != null)
        {
            modalCloseButton.clicked += HideHowItWorksModal;
        }

        if (modalStartButton != null)
        {
            modalStartButton.clicked += onStartExperience;
        }
    }

    private void ShowHowItWorksModal()
    {
        if (howItWorksModal == null)
        {
            Debug.LogWarning("[WelcomeViewController] No se encontró how-it-works-modal.");
            return;
        }

        howItWorksModal.RemoveFromClassList("hidden");
    }

    private void HideHowItWorksModal()
    {
        if (howItWorksModal == null)
        {
            return;
        }

        howItWorksModal.AddToClassList("hidden");
    }
}