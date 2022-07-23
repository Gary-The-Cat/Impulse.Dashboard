// <copyright file="UnregisteredServiceException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Impulse.Shared.Exceptions;
public class UnregisteredServiceException : Exception
{
    public UnregisteredServiceException(string serviceName) :
        base(serviceName) { }
}
