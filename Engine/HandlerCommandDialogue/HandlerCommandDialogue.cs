using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Engine
{
    public static class HandlerCommandDialogue
    {
        private const string nameParentObjectHelpTextBoxes = "___helpTextBoxesOfDialogueText___";

        /// <summary>
        /// Maker all of commands in dialogue
        /// </summary>
        /// <param name="global_Control">global control which control text of dialogue</param>
        /// <param name="dialogueText">dialogue</param>
        public static void MakeCommands(Global_control global_Control, Scene_class.DialogueText dialogueText)
        {
            foreach (Scene_class.DialogueText.CommandDialogue commandDialogue in dialogueText.commandsDialogue)
            {
                switch (commandDialogue.nameCommand)
                {
                    case "helpTextBox":
                        HelpTextBoxCommand(global_Control, commandDialogue);
                        break;

                    default:
                        DebugEngine.LogWarning($"HandlerCommandDialogue:MakeCommands: Unknown dialogue command: {commandDialogue.nameCommand}");
                        break;
                }
            }
        }

        /// <summary>
        /// Finish or delete all of commands in dialogue
        /// </summary>
        /// <param name="global_Control">global control which control text of dialogue</param>
        /// <param name="dialogueText">dialogue</param>
        public static void FinishCommands(Global_control global_Control, Scene_class.DialogueText dialogueText)
        {
            foreach (Scene_class.DialogueText.CommandDialogue commandDialogue in dialogueText.commandsDialogue)
            {
                switch (commandDialogue.nameCommand)
                {
                    case "helpTextBox":
                        AllHelpTextBoxesDelete(global_Control);
                        break;

                    default:
                        DebugEngine.LogWarning($"HandlerCommandDialogue:MakeCommands: Unknown dialogue command: {commandDialogue.nameCommand}");
                        break;
                }
            }
        }

        /// <summary>
        /// Maker of help text box command in dialogue
        /// </summary>
        /// <param name="global_Control">global control which control text of dialogue</param>
        /// <param name="commandDialogue">command dialogue</param>
        public static void HelpTextBoxCommand(Global_control global_Control, Scene_class.DialogueText.CommandDialogue commandDialogue)
        {
            if (commandDialogue == null)
            {
                throw new Exception("HandlerCommandDialogue:HelpTextBoxCommand: commandDialogue is null!");
            }
            if (commandDialogue.nameCommand != "helpTextBox")
            {
                throw new Exception("HandlerCommandDialogue:HelpTextBoxCommand: commandDialogue has a wrong name!");
            }
            if (global_Control == null || global_Control.text_dialogue == null)
            {
                throw new Exception("HandlerCommandDialogue:HelpTextBoxCommand: global control or text dialogue is null!");
            }

            GameObject parentBoxes = null;
            if (global_Control.text_dialogue.transform.GetChild(global_Control.text_dialogue.transform.childCount - 1).name != nameParentObjectHelpTextBoxes)
            {
                parentBoxes = new GameObject(nameParentObjectHelpTextBoxes);
                parentBoxes.transform.SetParent(global_Control.text_dialogue.transform);
                parentBoxes.transform.SetSiblingIndex(global_Control.text_dialogue.transform.childCount);
            }
            else
            {
                parentBoxes = global_Control.text_dialogue.transform.GetChild(global_Control.text_dialogue.transform.childCount - 1).gameObject;
            }

            TextHelpBoxPrefabsController textHelpBoxPrefabsController = global_Control.FindAllObjectsOfType<TextHelpBoxPrefabsController>()[0];
            GameObject prefabBox = null;

            if (commandDialogue.dictValuesString["nameBox"] != null && commandDialogue.dictValuesString["nameBox"].Length > 0)
            {
                textHelpBoxPrefabsController.Get(commandDialogue.dictValuesString["nameBox"][0]);
            }

            if (prefabBox == null)
            {
                prefabBox = textHelpBoxPrefabsController.Get(0);
            }
            if (prefabBox == null)
            {
                throw new Exception("HandlerCommandDialogue:HelpTextBoxCommand: prefab box is null!");
            }

            string message = null;
            if (commandDialogue.dictValuesString["message"] != null && commandDialogue.dictValuesString["message"].Length > 0)
            {
                message = commandDialogue.dictValuesString["message"][0];
            }
            if (message == null)
            {
                message = "";
            }

            int indexStart = 0, indexEnd = -1;
            if (commandDialogue.dictValues["indexes"] != null && commandDialogue.dictValues["indexes"].Length > 1)
            {
                indexStart = Convert.ToInt32(commandDialogue.dictValues["indexes"][0]);
                indexEnd = Convert.ToInt32(commandDialogue.dictValues["indexes"][1]);
            }

            Global_control.SpawnObject(prefabBox, prefabBox.name, parentBoxes.transform).GetComponent<TextHelpBox>().Init(global_Control.text_dialogue, message, indexStart, indexEnd);
        }

        /// <summary>
        /// Delete all help text boxes attached to text of dialogue
        /// </summary>
        /// <param name="global_Control">global control which control text of dialogue</param>
        public static void AllHelpTextBoxesDelete(Global_control global_Control)
        {
            if (global_Control == null || global_Control.text_dialogue == null)
            {
                throw new Exception("HandlerCommandDialogue:AllHelpTextBoxesDelete: global control or text dialogue is null!");
            }

            for (int indexChild = 0; indexChild < global_Control.text_dialogue.transform.childCount; indexChild++)
            {
                GameObject child = global_Control.text_dialogue.transform.GetChild(indexChild).gameObject;

                if (child.name == nameParentObjectHelpTextBoxes)
                {
                    Global_control.Destroy(child);
                }
            }
        }
    }
}
