using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerCommandScene
{
    public HandlerCommandScene()
    {

    }

    public void SetCommand(Global_control global_Control, Scene_class.DialogueOrChoiceOrCommand command)
    {
        switch (command.type)
        {
            case 0:
                this.HandleDialogue(global_Control, command.dialogue);
                break;

            case 1:
                this.HandleChoice(global_Control, command.choice);
                break;

            case 2:
                this.HandleCommand(global_Control, command.command);
                break;

            default:
                break;
        }
    }

    private void HandleCommand(Global_control global_Control, Scene_class.Command command)
    {

    }

    private void HandleDialogue(Global_control global_Control, Scene_class.DialogueText dialogueText)
    {

    }

    private void HandleChoice(Global_control global_Control, Scene_class.ChoiceText choiceText)
    {

    }
}
