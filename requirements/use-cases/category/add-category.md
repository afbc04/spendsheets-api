# Summary

- **Title:** Add Category
- **Description:** Administrators can create categories
- **Pre-condition:** User must be Administrator
- **Pos-condition:** System has a new category

# Flow

### Normal Flow

1. User initiates the add category process.
2. User provides:
    - Name
    - Description _(Optional)_
3. System validates the information provided
4. System determines the ID and Creation Date of new category
5. System saves the new category, providing the ID to the user 

### Exception (3) [Information is invalid]

4. System informs to user that information provided is invalid
