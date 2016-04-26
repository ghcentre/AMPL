using System;

namespace Ampl.Tasks
{
  /// <summary>
  /// Task status.
  /// </summary>
  public enum TaskStatus
  {
    /// <summary>
    /// New task.
    /// </summary>
    New,

    /// <summary>
    /// Task is queued.
    /// </summary>
    Queued,

    /// <summary>
    /// Task is in progress.
    /// </summary>
    Progress,

    /// <summary>
    /// Task completed.
    /// </summary>
    Complete,

    /// <summary>
    /// Task completed with error.
    /// </summary>
    Error,

    /// <summary>
    /// Task skipped.
    /// </summary>
    Skipped,
  }
}
