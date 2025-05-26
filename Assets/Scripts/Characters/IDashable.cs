public interface IDashable {
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
    public void StartDash();
    public void StartDashCooldown();
}
