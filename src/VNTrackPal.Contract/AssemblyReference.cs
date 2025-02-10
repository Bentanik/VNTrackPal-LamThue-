using System.Reflection;

namespace VNTrackPal.Contract;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
