using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MyGameFrameWork;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MyGameFrameWork {
    public class BasePanel : WindowBehaviour
    {
        private List<Button> mButtonList = new List<Button>();

        private List<Toggle> mToggleList = new List<Toggle>();

        private List<InputField> mInputFieldList = new List<InputField>();

        private CanvasGroup mUIMask;

        private CanvasGroup mCanvasGroup;

        protected Transform mUIContent;

        protected bool mDisableAnim = false;//���ö���

        public E_UI_Layer _Layer;

        /// <summary>
        /// ��ʼ���������
        /// </summary>
        private void InitializeBaseComponent()
        {
            mCanvasGroup = transform.GetComponent<CanvasGroup>();
            mUIMask = transform.Find("UIMask").GetComponent<CanvasGroup>();
            mUIContent = transform.Find("UIContent").transform;
        }

        /// <summary>
        /// ��ť����¼�
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="action"></param>
        public void AddButtonClickListener(Button btn, UnityAction action)
        {
            if (btn != null)
            {
                if (!mButtonList.Contains(btn))
                {
                    mButtonList.Add(btn);
                }
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(action);
            }
        }
        /// <summary>
        /// Toggle����¼�
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="action"></param>
        public void AddToggleClickListener(Toggle toggle, UnityAction<bool, Toggle> action)
        {
            if (toggle != null)
            {
                if (!mToggleList.Contains(toggle))
                {
                    mToggleList.Add(toggle);
                }
                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener((isOn) =>
                {
                    action?.Invoke(isOn, toggle);
                });
            }
        }
        /// <summary>
        /// InputField����¼�
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="action"></param>
        public void AddInputFieldClickListener(InputField inputField, UnityAction<string> onChangeAction, UnityAction<string> endInputAction)
        {
            if (inputField != null)
            {
                if (!mInputFieldList.Contains(inputField))
                {
                    mInputFieldList.Add(inputField);
                }
                inputField.onValueChanged.RemoveAllListeners();
                inputField.onEndEdit.RemoveAllListeners();
                inputField.onValueChanged.AddListener(onChangeAction);
                inputField.onEndEdit.AddListener(endInputAction);
            }
        }

        /// <summary>
        /// �õ���Ӧ���������ֵĶ�Ӧ�ؼ��ű�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controlName"></param>
        /// <returns></returns>
        protected T GetControl<T>(string controlName) where T : Component
        {
            Transform trans = this.gameObject.transform.FindMyChild(controlName);
            Component component = trans.GetComponent<T>();
            if (component != null)
            {
                return component as T;
            }
            return null;
        }
        /// <summary>
        /// �����ڶఴť
        /// </summary>
        /// <param name="btnName"></param>
        protected virtual void OnClick(string btnName)
        {

        }
        /// <summary>
        /// �����ڶఴť
        /// </summary>
        /// <param name="btnName"></param>
        protected virtual void OnClick(int btnIndex)
        {

        }
        /// <summary>
        /// �����ڶ�Toggel
        /// </summary>
        /// <param name="btnName"></param>
        protected virtual void OnValueChanged(bool value, string toggleName)
        {

        }
        /// <summary>
        /// �����ڶ�Toggel
        /// </summary>
        /// <param name="btnName"></param>
        protected virtual void OnValueChanged(bool value, int index)
        {

        }
        /// <summary>
        /// �����ڶ�Slider
        /// </summary>
        /// <param name="btnName"></param>
        protected virtual void OnValueChanged(float floatvalue, int index)
        {

        }

        public override void Init()
        {
            base.Init();
            InitializeBaseComponent();
        }
        public override void ShowMe()
        {
            base.ShowMe();
            ShowAnimation();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void HideMe()
        {
            base.HideMe();
            HideAnimation();
        }

        /// <summary>
        /// �򿪴��ڶ���
        /// </summary>
        public void ShowAnimation()
        {
            //������������Ҫ����
            if (canvas.sortingOrder > 90 && mDisableAnim == false)
            {
                //Mask����
                mUIMask.alpha = 0;
                mUIMask.DOFade(1, 0.2f);
                //���Ŷ���
                mUIContent.localScale = Vector3.one * 0.8f;
                mUIContent.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }
        public void HideAnimation()
        {
            if (canvas.sortingOrder > 90 && mDisableAnim == false)
            {
                mUIContent.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    UIManager.GetInstance().HidePanel(name);
                });
            }
            else
            {
                UIManager.GetInstance().HidePanel(name);
            }
        }
        public override void SetActive(bool Active)
        {
            base.SetActive(Active);
            //gameObject.SetActive(Active);
            mCanvasGroup.alpha = Active ? 1 : 0;
            mCanvasGroup.blocksRaycasts = Active;
            mCanvasGroup.interactable = Active;
            isActive = Active;
        }
        public void SetMaskVisible(bool isVisble)
        {
            if (!UISetting.Instance.SINGMASK_SYSTEM)
            {
                return;
            }
            mUIMask.alpha = isVisble ? 1 : 0;
        }
        /// <summary>
        /// �Ƴ����а�ť����
        /// </summary>
        public void RemoveAllButtonListener()
        {
            foreach (var item in mButtonList)
            {
                item.onClick.RemoveAllListeners();
            }
        }
        /// <summary>
        /// �Ƴ�����Toggle����
        /// </summary>
        public void RemoveAllToggleListener()
        {
            foreach (var item in mToggleList)
            {
                item.onValueChanged.RemoveAllListeners();
            }
        }
        /// <summary>
        /// �Ƴ�����InputField����
        /// </summary>
        public void RemoveAllInputFieldListener()
        {
            foreach (var item in mInputFieldList)
            {
                item.onValueChanged.RemoveAllListeners();
                item.onEndEdit.RemoveAllListeners();
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            RemoveAllButtonListener();
            RemoveAllInputFieldListener();
            RemoveAllToggleListener();
            mButtonList.Clear();
            mToggleList.Clear();
            mInputFieldList.Clear();
        }
    }
}
