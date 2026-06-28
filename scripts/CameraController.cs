using Godot;
using System;

public partial class CameraController : SpringArm3D
{
	[Export] private Camera3D _camera;
	[Export(PropertyHint.Range, "0.01,1,0.01")] private float _mouseSensitivity = 0.5f;
	
	private Vector2 _cameraInputDirection = Vector2.Zero;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		cameraRotation.X -= this._cameraInputDirection.Y * (float)delta; // Pitch
		cameraRotation.Y -= this._cameraInputDirection.X * (float)delta; // Yaw
		
		// Min and Max Pitch Rotations to -+89 (because 90 causes Backward Movement)
		float minPitchRotation = (-89f * Single.Pi) / 180f; // -89 Degrees
		float maxPitchRotation = (89f * Single.Pi) / 180f; // 89 Degrees
		cameraRotation.X = Mathf.Clamp(cameraRotation.X, minPitchRotation, maxPitchRotation);
		
		// Apply the Camera Movement
		this.Rotation = cameraRotation;
		this._cameraInputDirection = Vector2.Zero; // Reset the Camera Input Direction
	}
	
	public override void _Input(InputEvent @event)
	{
		// Event was a Key Press
		if (@event is InputEventKey key)
		{
			// Key was Pressed
			if (key.Pressed)
			{
				// Quit Game
				if (key.IsActionPressed("QuitGame"))
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
			this._cameraInputDirection = mouseMotion.ScreenRelative * this._mouseSensitivity;
		}
	}
}
