# How to Add a New Property to an Entity (and Add a Column to a Table)

1. Open the _entity_ to which you want to add the property (e.g., `User.cs`).
2. Add a new property (e.g., `string Email`). Ensure that the setter is private.
3. Open the _entity configuration_ class (e.g., `UserConfiguration.cs`). This class contains the database mapping information.
4. Adjust the table/column mapping as needed (e.g., null/non-null constraints, unique key constraints, indexes, default values, data type, and max length).
   - Try to avoid adding configurations for properties/columns where Entity Framework can derive the information from the property metadata (e.g., if a property is non-nullable, then a non-null column will automatically be generated. There is no need to specify non-nullability manually in the configuration).
5. Execute the script: [_add-migration.ps1_](../../scripts/add-migration.ps1).
6. Provide a meaningful name for the migration (e.g., `Alter<EntityName>Add<PropertyName>`).
7. Review the newly generated migration file:
   - If the migration needs adjustment:
      - Delete the migration.
      - Revert the changes to the `AppDbContextModelSnapshot.cs`.
      - Correct the entity or the entity configuration file and then regenerate the migration.
   - If the migration is correct:
      - Extend the places where the new property needs to be used (e.g., create or update methods, builder classes).
      - Ensure that validations are in place for the new property.
      - Adjust the integration tests accordingly.
8. Extend the seeding logic for this entity if applicable.