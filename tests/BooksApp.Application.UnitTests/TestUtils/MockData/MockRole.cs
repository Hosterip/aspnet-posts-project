﻿using PostsApp.Domain.Role;

namespace Application.UnitTest.TestUtils.MockData;

public static class MockRole
{
    public static Role GetRole(string role) => Role.Create(role);
}