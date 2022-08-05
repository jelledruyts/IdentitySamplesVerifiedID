using System;
using MicrosoftEntra.VerifiedId.Client.Models;

namespace MicrosoftEntra.VerifiedId.Client;

[System.Serializable]
public class VerifiedIdException : Exception
{
    public ErrorResponse ErrorResponse { get; }

    public VerifiedIdException(ErrorResponse errorResponse)
        : this(errorResponse, null, null)
    {
    }

    public VerifiedIdException(ErrorResponse errorResponse, string message)
        : this(errorResponse, message, null)
    {
    }

    public VerifiedIdException(ErrorResponse errorResponse, string? message, Exception? inner)
        : base(message, inner)
    {
        this.ErrorResponse = errorResponse;
    }
}