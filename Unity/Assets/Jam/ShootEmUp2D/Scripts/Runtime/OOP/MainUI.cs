using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The View handles user interface and user input
    /// </summary>
    public class MainUI : MonoBehaviour
    {
        //  Events ----------------------------------------
        public readonly UnityEvent OnRestartRequest = new UnityEvent();
        public readonly UnityEvent OnSwitchWeaponOne = new UnityEvent();
        public readonly UnityEvent OnSwitchWeaponTwo = new UnityEvent();
        public readonly UnityEvent OnSwitchWeaponThree = new UnityEvent();
        public readonly UnityEvent OnRestartConfirm = new UnityEvent();
        public readonly UnityEvent OnRestartCancel = new UnityEvent();

        //  Properties ------------------------------------
        public Label ScoreLabel { get { return _uiDocument?.rootVisualElement?.Q<Label>("ScoreLabel");} }
        public Label StatusLabel { get { return _uiDocument?.rootVisualElement?.Q<Label>("StatusLabel");} }
        public Label WaveTitleLabel { get { return _uiDocument?.rootVisualElement?.Q<Label>("WaveTitleLabel");} }
        public Label WaveProgressLabel { get { return _uiDocument?.rootVisualElement?.Q<Label>("WaveProgressLabel");} }
        public Button RestartButton { get { return _uiDocument?.rootVisualElement?.Q<Button>("RestartButton");} }
        public Button WeaponButtonOne { get { return _uiDocument?.rootVisualElement?.Q<Button>("WeaponButtonOne"); } }
        public Button WeaponButtonTwo { get { return _uiDocument?.rootVisualElement?.Q<Button>("WeaponButtonTwo"); } }
        public Button WeaponButtonThree { get { return _uiDocument?.rootVisualElement?.Q<Button>("WeaponButtonThree"); } }

        //  Fields ----------------------------------------
        [SerializeField] 
        private UIDocument _uiDocument;
        
        private DialogVisualElement _dialogVisualElement;

        //  Unity Methods  -------------------------------
        protected void Start()
        {
            VisualElement dialogVisualElement = _uiDocument.rootVisualElement.Q<VisualElement>("DialogVisualElement"); 
            _dialogVisualElement = new DialogVisualElement(dialogVisualElement);
            _dialogVisualElement.OnConfirm.AddListener(DialogUI_OnConfirm);
            _dialogVisualElement.OnCancel.AddListener(DialogUI_OnCancel);
            _dialogVisualElement.BodyLabel.text = "Are You Sure?";
            _dialogVisualElement.IsVisible = false;
            
            RestartButton.clicked += RestartButton_OnClicked;
            
            _uiDocument.rootVisualElement.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Space ||
                    evt.keyCode == KeyCode.Return ||
                    evt.keyCode == KeyCode.KeypadEnter
                    )
                {
                    evt.StopImmediatePropagation();
                }
            }, TrickleDown.TrickleDown);
          
            WeaponButtonOne.clicked += SwitchWeaponOne_OnClicked;
            WeaponButtonTwo.clicked += SwitchWeaponTwo_OnClicked;
            WeaponButtonThree.clicked += SwitchWeaponThree_OnClicked;
        }

        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
        private void RestartButton_OnClicked()
        {
            _dialogVisualElement.IsVisible = true;
            OnRestartRequest.Invoke();
        }

        private void SwitchWeaponOne_OnClicked()
        {
            OnSwitchWeaponOne.Invoke();
        }

        private void SwitchWeaponTwo_OnClicked()
        {
            OnSwitchWeaponTwo.Invoke();
        }

        private void SwitchWeaponThree_OnClicked()
        {
            OnSwitchWeaponThree.Invoke();
        }

        private void DialogUI_OnConfirm()
        {
            _dialogVisualElement.IsVisible = false;
            OnRestartConfirm.Invoke();
        }
        
        private void DialogUI_OnCancel()
        {
            _dialogVisualElement.IsVisible = false;
            OnRestartCancel.Invoke();
        }
    }
}