using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameFrameWork
{
    public interface CommandBase
    {
        /// <summary>
        /// ִ������
        /// </summary>
        void Excute();


        /// <summary>
        /// ��������
        /// </summary>
        void Undo();
    }
}
