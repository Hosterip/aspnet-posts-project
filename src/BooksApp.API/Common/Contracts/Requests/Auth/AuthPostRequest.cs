﻿namespace PostsApp.Common.Contracts.Requests.Auth;

public class AuthPostRequest
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}