using Godot;
using System;

public partial class CameraController : SpringArm3D
{
	[Export] private Camera3D _camera;
	[Export(PropertyHint.Range, "0.01,1,0.01")] private float _mouseSensitivity = 0.5f;
	
	// For Camera Movement Inputs
	private Vector2 _cameraInputDirection = Vector2.Zero;
	
	// For Zooming In and Out
	private const float MinSpringArmLength = 0.2f;
	private const float MaxSpringArmLength = 5.0f;
	
	// Parent Node
	private Node3D _parent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set the Mouse Input Mode to Capture
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		
		// For Ignoring the Parent Rotation
		this._parent = this.GetParent<Node3D>();
		this.TopLevel = true;
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
		float maxPitchRotation = (45 * Single.Pi) / 180f; // 45 Degrees
		cameraRotation.X = Mathf.Clamp(cameraRotation.X, minPitchRotation, maxPitchRotation);
		
		// Apply the Camera Movement
		this.Rotation = cameraRotation;
		this._cameraInputDirection = Vector2.Zero; // Reset the Camera Input Direction
	}

	public override void _PhysicsProcess(double delta)
	{
		// Follow the Parent Node's Position Transform only
		this.GlobalPosition = this._parent.GlobalPosition;
	}
	
	public override void _Input(InputEvent @event)
	{
		// Event was a Key Input
		if (@event is InputEventKey key)
		{
			// Key was Pressed
			if (key.Pressed)
			{
				// Exit Focus
				if (key.IsActionPressed("ExitFocus"))
				{
					Input.SetMouseMode(Input.MouseModeEnum.Visible);
				}
			}
		}
		
		// Event was a Mouse Input
		if (@event is InputEventMouse mouseInput)
		{
			// Mouse Input was a Mouse Button
			if (mouseInput is InputEventMouseButton mouseButton)
			{
				// Mouse Button was Pressed
				if (mouseButton.Pressed)
				{
					// Enter Focus
					if (mouseButton.IsActionPressed("EnterFocus"))
					{
						Input.SetMouseMode(Input.MouseModeEnum.Captured);
					}
				}
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Mouse Related Inputs
		if (@event is InputEventMouse)
		{
			// Check if the Mouse Input was a Mouse Button
			if (@event is InputEventMouseButton mouseButton)
			{
				// Scroll Wheel Up
				if (mouseButton.ButtonIndex == MouseButton.WheelUp)
				{
					// Zoom In and Decrease the Spring Arm Length
					this.SpringLength = (this.SpringLength <= MinSpringArmLength)
						? this.SpringLength : this.SpringLength - 0.1f;
				}
				
				// Scroll Wheel Down
				if (mouseButton.ButtonIndex == MouseButton.WheelDown)
				{
					// Zoom Out and Increase the Spring Arm Length
					this.SpringLength = (this.SpringLength >= MaxSpringArmLength)
						? this.SpringLength : this.SpringLength + 0.1f;
				}
			}
			
			// Check if the Mouse Input was a Mouse Movement
			if (@event is InputEventMouseMotion mouseMotion)
			{
				// Ensure that the Mouse Input Mode is set to Capture
				if (Input.GetMouseMode() != Input.MouseModeEnum.Captured) return;
			
				// Camera Movement Input
				this._cameraInputDirection = mouseMotion.ScreenRelative * this._mouseSensitivity;
			}
		}
	}
}
