namespace VNTrackPal.Contract.Settings;

public class EmailSetting
{
    public const string SectionName = "EmailSetting";
    public string EmailHost { get; set; } = default!;
    public string EmailUsername { get; set; } = default!;
    public string EmailPassword { get; set; } = default!;
}
