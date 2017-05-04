﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ampl.Configuration
{
  /// <summary>
  /// Provides methods for committing or rolling back transactions.
  /// </summary>
  public interface IAppConfigTransaction : IDisposable
  {
    /// <summary>
    /// Commits the transaction.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rolls back the transaction.
    /// </summary>
    void Rollback();
  }
}