using System;

namespace MyFw
{
    public class VersionValue : IComparable<VersionValue>
    {
        public readonly int major;
        public readonly int minor;
        public readonly int patch;

        public VersionValue(string versionString = "")
        {
            if (string.IsNullOrEmpty(versionString))
            {
                versionString = UnityEngine.Application.version;
            }

            var versionParts = versionString.Split('.');
            this.major = int.TryParse(versionParts[0], out var v) ? v : 0;
            this.minor = int.TryParse(versionParts[1], out var v1) ? v1 : 0;
            this.patch = int.TryParse(versionParts[2], out var v2) ? v2 : 0;
        }

        public override string ToString() => $"{this.major}.{this.minor}.{this.patch}";

        public int CompareTo(VersionValue other)
        {
            if (other == null)
            {
                return 1;
            }
            else if (this.major != other.major)
            {
                return this.major.CompareTo(other.major);
            }
            else if (this.minor != other.minor)
            {
                return minor.CompareTo(other.minor);
            }

            return this.patch.CompareTo(other.patch);
        }
    }
}