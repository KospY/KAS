﻿// Kerbal Attachment System
// Module author: igor.zavoychinskiy@gmail.com
// License: Public Domain

namespace KASAPIv2 {

/// <summary>A generic interface to deal with the vessels info.</summary>
public interface ILinkVesselInfo {

  /// <summary>Part that owns the info.</summary>
  /// <value>Instance of the part.</value>
  Part part { get; }

  /// <summary>The persisted vessel's info.</summary>
  /// <value>The vessel info or <c>null</c>.</value>
  DockedVesselInfo vesselInfo { get; set; }
}

}  // namespace
