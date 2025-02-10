using System.Reflection;

namespace VNTrackPal.Persistence;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
