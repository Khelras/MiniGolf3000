using Godot;

public partial class PlayerController : CharacterBody3D
{
	[ExportCategory("Movement")]
	[Export] public float Speed = 2f;
	[Export] public float Acceleration = 20f;
	[Export] public float JumpHeight = 4f;

	[ExportCategory("Camera")]
	[Export] private Camera3D _camera;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Ensure the Speed Property was Set Properly
		if (Speed <= 0f)
		{
			GD.PushWarning("Speed set incorrectly, defaulting to 2.");
			this.Speed = 2f;
		}
		
		// Ensure the Acceleration Property was Set Properly
		if (Acceleration <= 0f)
		{
			GD.PushWarning("Acceleration set incorrectly, defaulting to 20.");
			this.Acceleration = 20f;
		}
		
		// Ensure the Jump Height Property was Set Properly
		if (this.JumpHeight <= 0f)
		{
			GD.PushWarning("Jump height set incorrectly, defaulting to 4.");
			this.JumpHeight = 4f;
		}
		
		// Set the Mouse Input Mode to Capture
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// 3D Velocity Vector
		Vector3 velocity = this.Velocity;

		// Apply Gravity if the Player is not on the Floor
		if (!this.IsOnFloor()) velocity += this.GetGravity() * (float)delta;
		
		// Apply the Jump Height when the Player Jumps and was on the Floor
		if (Input.IsActionJustPressed("Jump") && this.IsOnFloor()) velocity.Y += this.JumpHeight;
		
		// Use the Camera's Forward and Right Vector for Movement to be Relative to the Camera
		Vector3 forward = this._camera.GlobalBasis.Z;
		Vector3 right = this._camera.GlobalBasis.X;
		
		// Get the Input Axis and apply the Forward and Right Vector for Movement Direction
		Vector2 inputAxis = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
		Vector3 movementDirection = (forward * inputAxis.Y) + (right * inputAxis.X);
		movementDirection.Y = 0f;
		movementDirection = movementDirection.Normalized();
		
		// Calculate the Velocity based on the Movement Direction and Speed
		velocity.X = Mathf.MoveToward(this.Velocity.X, movementDirection.X * this.Speed, this.Acceleration * (float)delta);
		velocity.Z = Mathf.MoveToward(this.Velocity.Z, movementDirection.Z * this.Speed, this.Acceleration * (float)delta);
		
		// Apply the Velocity
		this.Velocity = velocity;
		MoveAndSlide();
	}
}
