using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Engine 
{
    [Serializable]
    public enum TypesSkiping
    {
        None,
        AllText,
        ShowedText,
    }

    public static class TypesCommandsSkiped
    {
        public static Type[] GetTypes(TypesSkiping typeSkiping)
        {
            switch (typeSkiping)
            {
                case TypesSkiping.None:
                    return new Type[0];

                case TypesSkiping.AllText:
                    return new Type[] { typeof(Scene_class.DialogueText), typeof(Scene_class.Command) };

                case TypesSkiping.ShowedText:
                    return new Type[] { typeof(Scene_class.DialogueText), typeof(Scene_class.Command) };

                default:
                    return new Type[0];
            }
        }

        public static bool GetIsShowedSkip(TypesSkiping typeSkiping)
        {
            switch (typeSkiping)
            {
                case TypesSkiping.None:
                    return true;

                case TypesSkiping.AllText:
                    return false;

                case TypesSkiping.ShowedText:
                    return true;

                default:
                    return true;
            }
        }
    }
}
