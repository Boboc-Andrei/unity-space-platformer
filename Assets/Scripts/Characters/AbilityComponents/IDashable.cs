public interface IDashable {
    public bool IsActive { get; set; }
    public bool IsOffCooldown { get; set; }
    public void StartDash(int direction);
    public void StartDashCooldown();
}
