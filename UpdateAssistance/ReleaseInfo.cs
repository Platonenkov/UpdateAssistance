using System.Collections.Generic;
using UpdateAssistance.Documents;

namespace UpdateAssistance
{
    /// <summary>
    /// <see cref="ReleaseInfo"/> describes a specific version of th GMDC application.
    /// </summary>
    public class ReleaseInfo
    {
        /// <summary>
        /// Gets or sets the version string for this release.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the release notes for this release.
        /// </summary>
        public string ReleaseNotesText { get; set; }

        /// <summary>
        /// Gets or sets the formatted release notes for this release.
        /// </summary>
        public IEnumerable<Inline> ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this version is a pre-release.
        /// </summary>
        public bool PreRelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this release is the newest.
        /// </summary>
        public bool IsLatest { get; set; }
    }
}