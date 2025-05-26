public interface IJumpable {
    public bool IsJumping { get; set; }
    public void Jump();
    public bool CanJump();
    public bool CanCoyoteJump();

}
