using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameFrameWork
{
    public interface CommandBase
    {
        /// <summary>
        /// Ö´ÐÐÃüÁî
        /// </summary>
        void Excute();


        /// <summary>
        /// ³·ÏúÃüÁî
        /// </summary>
        void Undo();
    }
}
