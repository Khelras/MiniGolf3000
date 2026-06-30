using Godot;
using System;

public partial class PlayerGolfController : RigidBody3D
{
	[ExportCategory("Physics")]
	[Export] public float ForceStrength = 10f;
	[Export] public float Torque = 20f;

	[ExportCategory("Camera")]
	[Export] private Camera3D _camera;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		// Use the Camera's Forward and Right Vector for Movement to be Relative to the Camera
		Vector3 forward = this._camera.GlobalBasis.Z;
		Vector3 right = this._camera.GlobalBasis.X;
		
		// Get the Input Axis and apply the Forward and Right Vector for Movement Direction
		Vector2 inputAxis = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
		Vector3 movementDirection = (forward * inputAxis.Y) + (right * inputAxis.X);
		movementDirection.Y = 0f;
		movementDirection = movementDirection.Normalized();
		
		// Applying the Force
		state.ApplyCentralForce(movementDirection * ForceStrength);
	}
}
