using System;
using UnityEngine.UI;

public class ActionButton : Button {

    private Action pressedHandler;

    void Update () {
        if (IsPressed ()) {
            this.pressedHandler?.Invoke ();
        }
    }

    public void registerPressed (Action pressedAction) {
        this.pressedHandler = pressedAction;
    }

    public void unRegisterPressed () {
        this.pressedHandler = null;
    }

}