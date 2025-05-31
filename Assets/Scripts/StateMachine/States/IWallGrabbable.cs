public interface IWallGrabbable {
    public bool IsEnabled { get; set; }
    public float CurrentStamina { get; set; }
    public float MaxStamina { get; set; }
    public void StartCooldown();
    public void ResetStamina();
    public void Grab();
    public void Slide();
    public void Release();
}
