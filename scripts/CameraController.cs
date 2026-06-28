using Godot;
using System;

public partial class CameraController : SpringArm3D
{
	[Export] private Camera3D camera;
	[Export(PropertyHint.Range, "0.01,1,0.01")] private float mouseSensitivity = 0.5f;
	
	Vector2 cameraInputDirection = Vector2.Zero;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (this.camera == null) GD.PushError("Camera reference was not set");
		
		// Set the Mouse Input Mode to Capture
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Ensure that the Mouse Input Mode is set to Capture
		if (Input.GetMouseMode() != Input.MouseModeEnum.Captured) return;
		
		// Camera Movement Input Rotations
		Vector3 cameraRotation = this.Rotation;
		cameraRotation.X -= this.cameraInputDirection.Y * (float)delta; // Pitch
		cameraRotation.X = Mathf.Clamp(cameraRotation.X, -Single.Pi / 2f, Single.Pi / 2f);
		cameraRotation.Y -= this.cameraInputDirection.X * (float)delta; // Yaw
		this.cameraInputDirection = Vector2.Zero;
		
		// Apply the Camera Movement
		this.Rotation = cameraRotation;
	}
	
	public override void _Input(InputEvent @event)
	{
		// Event was a Key Press
		if (@event is InputEventKey key)
		{
			// Key was Pressed
			if (key.Pressed == true)
			{
				// Quit Game
				if (key.IsActionPressed("QuitGame") == true)
				{
					this.GetTree().Quit();
				}
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Check if the Mouse has been Moved
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// Ensure that the Mouse Input Mode is set to Capture
			if (Input.GetMouseMode() != Input.MouseModeEnum.Captured) return;
			
			// Camera Movement Input
			this.cameraInputDirection = mouseMotion.ScreenRelative * this.mouseSensitivity;
		}
	}
}
