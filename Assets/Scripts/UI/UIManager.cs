using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI�㼶
/// </summary>
public enum E_UI_Layer
{
    BottomPanelLayer,
    levelOneLayer,
    levelTwoLayer,
    levelThreeLayer,
}

namespace MyGameFrameWork
{
    public class UIManager : MyGameFrameWork.Singleton<UIManager>
    {
        private Camera mUICamera;

        private Transform mUIRoot;

        private LoadPrefabPathConfig loadPrefabPathConfig;

        private ABNameConfig aBNameConfig;

        private Dictionary<string, BasePanel> mAllPanelDic = new Dictionary<string, BasePanel>();

        private List<BasePanel> mAllPanelList = new List<BasePanel>();

        private List<BasePanel> mVisiblePanelList = new List<BasePanel>();

        private Queue<BasePanel> mPanelStack = new Queue<BasePanel>();

        private bool mStartPopStackWndStatus = false;

        /// <summary>
        /// levelԽ��㼶Խ��
        /// </summary>
        private Transform bottomPanelLayer;
        private Transform levelOneLayer;//sortingLayer 1-100
        private Transform levelTwoLayer;//sortingLayer 101-200
        private Transform levelThreeLayer;//sortingLayer 201-300

        public void Initialize()
        {
            mUICamera = Camera.main;
            mUIRoot = GameObject.Find("UIRoot").transform;
            bottomPanelLayer = new GameObject("BottomPanelLayer").transform;
            bottomPanelLayer.SetParent(mUIRoot);
            levelOneLayer = new GameObject("levelOneLayer").transform;
            levelOneLayer.SetParent(mUIRoot);
            levelTwoLayer = new GameObject("levelTwoLayer").transform;
            levelTwoLayer.SetParent(mUIRoot);
            levelThreeLayer = new GameObject("levelThreeLayer").transform;
            levelThreeLayer.SetParent(mUIRoot);
            loadPrefabPathConfig = Resources.Load<LoadPrefabPathConfig>("ScriptableObject/LoadPrefabPathConfig");
            aBNameConfig = Resources.Load<ABNameConfig>("ScriptableObject/ABNameConfig");
            //���ֻ��ϲ��ᴥ������
#if UNITY_EDITOR
            loadPrefabPathConfig.GeneratorPanelConfig();
            aBNameConfig.GeneratorABConfig();
#endif
        }

        /// <summary>
        /// Ԥ���أ�ֻ�������壬��������������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void PreLoadPanel<T>() where T : BasePanel, new()
        {
            System.Type type = typeof(T);
            string wndName = type.Name;
            T PanelBase = new T();
            //��¡���棬��ʼ��������Ϣ
            //1.���ɶ�Ӧ�Ĵ���Ԥ����
            GameObject nWnd = TempLoadPanel(wndName);
            //2.��ʼ����Ӧ������
            if (nWnd != null)
            {
                PanelBase.gameObject = nWnd;
                PanelBase.transform = nWnd.transform;
                PanelBase.canvas = nWnd.GetComponent<Canvas>();
                PanelBase.canvas.worldCamera = mUICamera;
                PanelBase.name = nWnd.name;
                PanelBase.Init();
                PanelBase.SetActive(false);
                RectTransform rectTrans = nWnd.GetComponent<RectTransform>();
                rectTrans.anchorMax = Vector2.one;
                rectTrans.offsetMax = Vector2.zero;
                rectTrans.offsetMin = Vector2.zero;
                mAllPanelDic.Add(wndName, PanelBase);
                mAllPanelList.Add(PanelBase);
            }
            Debug.Log("Ԥ���ش��� �������֣�" + wndName);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PopUpPanel<T>(E_UI_Layer e_UI_Layer) where T : BasePanel, new()
        {
            System.Type type = typeof(T);
            string wndName = type.Name;
            BasePanel wnd = GetPanel(wndName);
            if (wnd != null)
            {
                return ShowPanel(wndName) as T;
            }

            T t = new T();
            return InitializePanel(t, wndName, e_UI_Layer) as T;
        }

        /// <summary>
        /// ��ջϵͳ����
        /// </summary>
        /// <param name="Panel"></param>
        /// <returns></returns>
        private BasePanel PopUpPanel(BasePanel Panel, E_UI_Layer e_UI_Layer)
        {
            System.Type type = Panel.GetType();
            string wndName = type.Name;
            BasePanel wnd = GetPanel(wndName);
            if (wnd != null)
            {
                return ShowPanel(wndName);
            }
            return InitializePanel(Panel, wndName, e_UI_Layer);
        }
        private BasePanel InitializePanel(BasePanel PanelBase, string wndName, E_UI_Layer e_UI_Layer)
        {
            //1.���ɶ�Ӧ�Ĵ���Ԥ����
            GameObject nWnd = TempLoadPanel(wndName);
            //2.��ʼ����Ӧ������
            if (nWnd != null)
            {
                Transform father = bottomPanelLayer;
                switch (e_UI_Layer)
                {
                    case E_UI_Layer.BottomPanelLayer:
                        father = bottomPanelLayer;
                        break;
                    case E_UI_Layer.levelOneLayer:
                        father = levelOneLayer;
                        break;
                    case E_UI_Layer.levelTwoLayer:
                        father = levelTwoLayer;
                        break;
                    case E_UI_Layer.levelThreeLayer:
                        father = levelThreeLayer;
                        break;
                }
                PanelBase.gameObject = nWnd;
                PanelBase.transform = nWnd.transform;
                PanelBase.canvas = nWnd.GetComponent<Canvas>();
                PanelBase.canvas.worldCamera = mUICamera;
                PanelBase._Layer = e_UI_Layer;
                PanelBase.transform.SetParent(father);
                PanelBase.transform.SetAsLastSibling();
                PanelBase.name = nWnd.name;
                PanelBase.Init();
                PanelBase.SetActive(true);
                PanelBase.ShowMe();
                RectTransform rectTrans = nWnd.GetComponent<RectTransform>();
                rectTrans.anchorMax = Vector2.one;
                rectTrans.offsetMax = Vector2.zero;
                rectTrans.offsetMin = Vector2.zero;
                mAllPanelDic.Add(wndName, PanelBase);
                mAllPanelList.Add(PanelBase);
                mVisiblePanelList.Add(PanelBase);
                SetPanelMaskVisible();
                return PanelBase;
            }
            Debug.LogError("û�м��ص���Ӧ�Ĵ��� �������֣�" + wndName);
            return null;
        }
        private BasePanel GetPanel(string winName)
        {
            if (mAllPanelDic.ContainsKey(winName))
            {
                return mAllPanelDic[winName];
            }
            return null;
        }
        /// <summary>
        /// ��ȡ�Ѿ������ĵ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetPanel<T>() where T : BasePanel
        {
            System.Type type = typeof(T);
            foreach (var item in mVisiblePanelList)
            {
                if (item.name == type.Name)
                {
                    return (T)item;
                }
            }
            Debug.LogError("�ô���û�л�ȡ����" + type.Name);
            return null;
        }

        private BasePanel ShowPanel(string winName)
        {
            BasePanel Panel = null;
            if (mAllPanelDic.ContainsKey(winName))
            {
                Panel = mAllPanelDic[winName];
                if (Panel.gameObject != null && Panel.isActive == false)
                {
                    mVisiblePanelList.Add(Panel);
                    Panel.transform.SetAsLastSibling();
                    Panel.SetActive(true);
                    SetPanelMaskVisible();
                    Panel.ShowMe();
                }
                return Panel;
            }
            else
                Debug.LogError(winName + " ���ڲ�����");
            return null;
        }

        public void HidePanel(string wndName)
        {
            BasePanel Panel = GetPanel(wndName);
            HidePanel(Panel);
        }
        /// <summary>
        /// ���ؽ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void HidePanel<T>() where T : BasePanel
        {
            HidePanel(typeof(T).Name);
        }
        public void HidePanel(BasePanel Panel)
        {
            if (Panel != null && Panel.isActive)
            {
                mVisiblePanelList.Remove(Panel);
                Panel.SetActive(false);//���ص�������
                SetPanelMaskVisible();
                Panel.HideMe();
            }
            //�ڳ�ջ������£���һ����������ʱ���Զ���ջ�ֵ���һ������
            PopNextStackPanel(Panel);
        }
        private void DestroyPanel(string wndName)
        {
            BasePanel Panel = GetPanel(wndName);
            DestoryPanel(Panel);
        }
        public void DestroyWinodw<T>() where T : BasePanel
        {
            DestroyPanel(typeof(T).Name);
        }
        private void DestoryPanel(BasePanel Panel)
        {
            if (Panel != null)
            {
                if (mAllPanelDic.ContainsKey(Panel.name))
                {
                    mAllPanelDic.Remove(Panel.name);
                    mAllPanelList.Remove(Panel);
                    mVisiblePanelList.Remove(Panel);
                }
                Panel.SetActive(false);
                SetPanelMaskVisible();
                Panel.HideMe();
                Panel.OnDestroy();
                GameObject.Destroy(Panel.gameObject);
                //�ڳ�ջ������£���һ����������ʱ���Զ���ջ�ֵ���һ������
                PopNextStackPanel(Panel);
            }
        }
        /// <summary>
        /// �����˴����� ��������ȫ������
        /// </summary>
        /// <param name="filterlist"></param>
        public void DestroyAllPanel(List<string> filterlist = null)
        {
            for (int i = mAllPanelList.Count - 1; i >= 0; i--)
            {
                BasePanel Panel = mAllPanelList[i];
                if (Panel == null || (filterlist != null && filterlist.Contains(Panel.name)))
                {
                    continue;
                }
                DestroyPanel(Panel.name);
            }
            Resources.UnloadUnusedAssets();
        }
        /// <summary>
        /// ���õ�����
        /// </summary>
        private void SetPanelMaskVisible()
        {
            if (!UISetting.Instance.SINGMASK_SYSTEM)
            {
                return;
            }
            BasePanel MaxOrderPanel = null;//�����Ⱦ�㼶�Ĵ���
            int maxOrder = 0;//�����Ⱦ�㼶
            int maxIndex = 0;//��������±� ����ͬ���ڵ��µ�λ���±�
                             //1.�ر����д��ڵ�Mask ����Ϊ���ɼ�
                             //2.�����пɼ��������ҵ�һ���㼶���Ĵ��ڣ���Mask����Ϊ�ɼ�
            for (int i = 0; i < mVisiblePanelList.Count; i++)
            {
                BasePanel PanelBase = mVisiblePanelList[i];
                if (PanelBase != null && PanelBase.gameObject != null)
                {
                    PanelBase.SetMaskVisible(false);
                    if (MaxOrderPanel == null)
                    {
                        MaxOrderPanel = PanelBase;
                        maxOrder = PanelBase.canvas.sortingOrder;
                        maxIndex = PanelBase.transform.GetSiblingIndex();
                    }
                    else
                    {
                        //�ҵ������Ⱦ�㼶�Ĵ��ڣ��õ���
                        if (maxOrder < PanelBase.canvas.sortingOrder)
                        {
                            MaxOrderPanel = PanelBase;
                            maxOrder = PanelBase.canvas.sortingOrder;
                        }
                        //����������ڵ���Ⱦ�㼶��ͬ�����ҵ�ͬ�ڵ������һ�����壬������ȾMask
                        else if (maxOrder == PanelBase.canvas.sortingOrder && maxIndex < PanelBase.transform.GetSiblingIndex())
                        {
                            MaxOrderPanel = PanelBase;
                            maxIndex = PanelBase.transform.GetSiblingIndex();
                        }
                    }
                }
            }
            if (MaxOrderPanel != null)
            {
                MaxOrderPanel.SetMaskVisible(true);
            }
        }

        /// <summary>
        /// ʵ��������
        /// </summary>
        /// <param name="wndName"></param>
        /// <returns></returns>
        private GameObject TempLoadPanel(string wndName)
        {
            //�Լ���ʱҪ�ĳ�AB����Adreesable
            GameObject Panel = GameObject.Instantiate<GameObject>(ABManager.GetInstance().LoadRes<GameObject>(aBNameConfig.GetABName(wndName), wndName), mUIRoot);
            //Panel.transform.SetParent(mUIRoot);
            Panel.transform.localScale = Vector3.one;
            Panel.transform.localPosition = Vector3.zero;
            Panel.transform.rotation = Quaternion.identity;
            Panel.name = wndName;
            return Panel;
        }

        /// <summary>
        /// ��ջһ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="popCallBack"></param>
        public void PushPanelToStack<T>(E_UI_Layer e_UI_Layer,Action<BasePanel> popCallBack = null) where T : BasePanel, new()
        {
            BasePanel wndBase = null;
            try
            {
                wndBase = mAllPanelDic[typeof(T).Name];
            }
            catch (Exception e)
            {
                wndBase = new T();
                wndBase._Layer = e_UI_Layer;
            }
            //T wndBase = new T();
            wndBase.PopStackListener = popCallBack;
            mPanelStack.Enqueue(wndBase);
        }
        /// <summary>
        /// ������ջ�е�һ������
        /// </summary>
        public void StartPopFirstStackPanel()
        {
            if (mStartPopStackWndStatus == true) return;//���뵯ջ�����˲�����ȥ��ʾPanel ����ֻ��ѹջ
            mStartPopStackWndStatus = true;//������ʾPanel
            PopStackPanel();
        }

        /// <summary>
        /// ������ջ�е���һ������
        /// </summary>
        /// <param name="PanelBase"></param>
        private void PopNextStackPanel(BasePanel PanelBase)
        {
            if (PanelBase != null && mStartPopStackWndStatus && PanelBase.PopStack)
            {
                PanelBase.PopStack = false;
                PopStackPanel();
            }
        }
        /// <summary>
        /// ѹ�벢�ҵ�����ջ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="popCallBack"></param>
        public void PushAndPopStackPanel<T>(E_UI_Layer e_UI_Layer,Action<BasePanel> popCallBack = null) where T : BasePanel, new()
        {
            PushPanelToStack<T>(e_UI_Layer,popCallBack);
            StartPopFirstStackPanel();
        }

        /// <summary>
        /// ������ջ����
        /// </summary>
        /// <returns></returns>
        private bool PopStackPanel()
        {
            if (mPanelStack.Count > 0)
            {
                BasePanel Panel = mPanelStack.Dequeue();
                BasePanel popPanel = PopUpPanel(Panel,Panel._Layer);
                popPanel.PopStackListener = Panel.PopStackListener;
                popPanel.PopStack = true;
                popPanel.PopStackListener?.Invoke(popPanel);
                popPanel.PopStackListener = null;
                return true;
            }
            else
            {
                mStartPopStackWndStatus = false;
                return false;
            }
        }
        public void ClearStackPanels()
        {
            mPanelStack.Clear();
        }
    }
}
