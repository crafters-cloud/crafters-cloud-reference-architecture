-- an Example of how to seed additional users into the database (for testing purposes)
INSERT INTO [User](Id, EmailAddress, FirstName, LastName, RoleId, UserStatusId, CreatedById, UpdatedById, CreatedOn,
                   UpdatedOn)
VALUES ('BAC33D1A-255B-4572-8967-EED289963FB7', 'email-here@here.com', 'first-name-here', 'last-name-here', '028E686D-51DE-4DD9-91E9-DFB5DDDE97D0', 1, 'dfb44aa8-bfc9-4d95-8f45-ed6da241dcfc', 'dfb44aa8-bfc9-4d95-8f45-ed6da241dcfc', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)