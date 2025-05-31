public interface IDashable {
    public bool IsActive { get; set; }
    public bool IsEnabled { get; set; }
    public void StartDash();
    public void StartDashCooldown();
}
