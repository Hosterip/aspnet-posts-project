﻿using PostsApp.Common.Requirements;

namespace PostsApp.Common;

public static class AuthRequirements
{
    public static readonly AuthorizedRequirement Authorized = new();
    public static readonly NotAuthorizedRequirement NotAuthorized = new();
}