using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MyGameFrameWork
{
    [CreateAssetMenu(fileName = "ABNameConfig", menuName = "ABNameConfig", order = 0)]
    public class ABNameConfig : ScriptableObject
    {
        private string[] ABRootArr = new string[] { "Game", "Hall", "Window" };
        public List<ABObjcet> ABDataList = new List<ABObjcet>();

        public void GeneratorABConfig()
        {

            foreach (var item in ABRootArr)
            {
                //��ȡԤ�����ļ��ж�ȡ·��
                string floder = Application.dataPath + "/Prefab/" + item;
                //��ȡ�ļ����µ�����Prefab�ļ�
                string[] filePathArr = Directory.GetFiles(floder, "*.prefab", SearchOption.AllDirectories);
                foreach (var path in filePathArr)
                {
                    if (path.EndsWith(".meta"))
                    {
                        continue;
                    }

                    //��ȡԤ��������
                    string fileName = Path.GetFileNameWithoutExtension(path);

                    string assetsPath = path.Substring(path.IndexOf("Assets"));

                    AssetImporter importer = AssetImporter.GetAtPath(assetsPath);

                    ABObjcet aBObjcet = new ABObjcet();
                    if (importer.assetBundleName != null)
                    {
                        aBObjcet.ABName = importer.assetBundleName;
                        aBObjcet.ResName = fileName;
                    }
                    ABDataList.Add(aBObjcet);
                }
            }
        }
        /// <summary>
        /// ͨ����Դ�����AB����
        /// </summary>
        /// <param name="resname"></param>
        public string GetABName(string resname)
        {
            for (int i = 0; i < ABDataList.Count; i++)
            {
                if (ABDataList[i].ResName == resname)
                {
                    if (ABDataList[i].ABName != "")
                    {
                        return ABDataList[i].ABName;
                    }
                }
            }

            return null;
        }
    }
    [System.Serializable]
    public class ABObjcet
    {
        public string ABName;
        public string ResName;
    }
}
