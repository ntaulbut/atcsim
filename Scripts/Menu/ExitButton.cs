using Godot;
using System;

public partial class ExitButton : Button
{
    public void OnPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
}
