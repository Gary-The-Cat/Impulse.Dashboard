using System;

namespace Impulse.Shared.Interfaces;

public interface IDateTimeProvider
{
    public virtual DateTime Now => DateTime.Now;
}
