using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class WorkingButtons : Scene_class
    {
        public float count;
        public string[] names;
        public ChangeFlag[][] changeFlag;
        public Command[][] commands;
        public WorkingButtons()
        {

        }
        public WorkingButtons(int count, string[] names, ChangeFlag[][] changeFlag, Command[][] commands)
        {
            this.count = count;
            this.names = names;
            this.changeFlag = changeFlag;
            this.commands = commands;
        }
        public WorkingButtons(int count_buttons)
        {
            this.names = new string[count_buttons];
            this.changeFlag = new ChangeFlag[count_buttons][];
            this.commands = new Command[count_buttons][];
        }

    }
}
