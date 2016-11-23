﻿// Kerbal Attachment System
// Mod's author: KospY (http://forum.kerbalspaceprogram.com/index.php?/profile/33868-kospy/)
// Module author: igor.zavoychinskiy@gmail.com
// License: Public Domain

using System;

namespace KASAPIv1 {

/// <summary>Defines global events that are triggered by KAS.</summary>
/// <remarks>Try to keep subscriptions to these events at the bare minimum. Too many listeners may
/// impact performance at the moment of actual event triggering.</remarks>
public static class KASEvents {
  /// <summary>A holder for simple source-to-target event.</summary>
  public struct LinkEvent {
    /// <summary>Link source.</summary>
    public readonly ILinkSource source;
    /// <summary>Link target.</summary>
    public readonly ILinkTarget target;
    /// <summary>Actor who changed the links tate.</summary>
    public LinkActorType actor;

    /// <summary>Creates an event info.</summary>
    /// <param name="source">Source that initiated the link.</param>
    /// <param name="target">Target that accepted the link.</param>
    /// <param name="actorType">
    /// Actor that did the change. <see cref="LinkActorType.API"/> by default.
    /// </param>
    public LinkEvent(ILinkSource source, ILinkTarget target,
                     LinkActorType actorType = LinkActorType.API) {
      this.source = source;
      this.target = target;
      this.actor = actorType;
    }
  }

  /// <summary>Triggers when a source has initiated linking mode.</summary>
  public static EventData<ILinkSource> OnStartLinking =
      new EventData<ILinkSource>("KASOnStartLinking");
  /// <summary>Triggers when source has stopped linking mode.</summary>
  public static EventData<ILinkSource> OnStopLinking =
      new EventData<ILinkSource>("KASOnStopLinking");
  /// <summary>Triggers when target has accepted the pending link.</summary>
  public static EventData<ILinkTarget> OnLinkAccepted =
      new EventData<ILinkTarget>("KASOnLinkAccepted");
  /// <summary>Triggers when link between two parts has been successfully established.</summary>
  /// <remarks>Consider using <see cref="ILinkStateEventListener.OnKASLinkCreatedEvent"/> when this
  /// state change is needed in scope of just one part.</remarks>
  public static EventData<LinkEvent> OnLinkCreated = new EventData<LinkEvent>("KASOnLinkCreated");
  /// <summary>Triggers when link between two parts has been broken.</summary>
  /// <remarks>Consider using <see cref="ILinkStateEventListener.OnKASLinkBrokenEvent"/> when this
  /// state change is needed in scope of just one part.</remarks>
  public static EventData<LinkEvent> OnLinkBroken = new EventData<LinkEvent>("KASOnLinkBroken");
}

}  // namespace
