﻿using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;

public class TestUserDataSeeding(DbContext context)
{
    public void Seed()
    {
        RecreateUser(TestUserData.CreateTestUser());
        RecreateUser(TestUserData.CreateSystemUser());
        context.SaveChanges();
    }

    private void RecreateUser(User user)
    {
        var usersSet = context.Set<User>();
        var fromDb = usersSet.Find(user.Id);
        if (fromDb == null)
        {
            usersSet.Add(user);
        }
    }
}