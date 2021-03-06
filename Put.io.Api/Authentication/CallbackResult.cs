﻿namespace Put.io.Api.Authentication
{
    public class CallbackResult
    {
        public CallbackStatus Status { get; set; }
        public string Token { get; set; }
    }

    public enum CallbackStatus
    {
        Success,
        Failed
    }
}