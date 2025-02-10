namespace VNTrackPal.Contract.Settings;

public class CloudinarySetting
{
    public const string SectionName = "CloudinarySetting";
    public string CloudName { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
    public string ApiSecret { get; set; } = default!;
    public string Folder { get; set; } = default!;
}
