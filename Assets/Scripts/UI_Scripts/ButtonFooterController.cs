using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



    public class ButtonFooterController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private Button footerBtn;
        [SerializeField] private bool lockOnAwake;

        [Header("Events")]
        public UnityEvent<ButtonFooterController> OnButtonClickedEvent;

        //Internal
        private bool selected;
        private bool locked;
        private void Awake()
        {
            SetLock(lockOnAwake);
        }

        private void Start()
        {
            footerBtn.onClick.AddListener(() =>
            {
                OnButtonClickedEvent?.Invoke(this);
            });
        }

        public void SetLock(bool locked)
        {
            locked = locked;

            footerBtn.interactable = locked == false;

            animator.SetBool("Locked", locked);
        }

        public void SetSelect(bool selected)
        {
            selected = selected;

            animator.SetBool("Selected", selected);
        }
    }

