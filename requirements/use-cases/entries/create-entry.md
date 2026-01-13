# Summary

- **Title:** Create Entry
- **Description:** Administrators can create entry
- **Pre-condition:** User must be Administrator
- **Pos-condition:** System has a new entry

# Flow

### Normal Flow

1. User initiates the create entry process.
2. User provides:
    - Type _(Transaction, Loan, Due Payment or Saving)_
    - Category _(Optional)_
    - Money (expected to be) removed or added into the wallet in the end _(Saving is always removed)_
    - Date _(Optional : Only applied in Transaction)_
    - Begin Date _(Optional)_
    - Scheduled Due Date _(Optional : Only applied in Multiple Payment or Saving Entries)_
    - List of tags _(Optional)_
    - Description _(Optional)_
    - Is it visible? _(Optional)_
    - Is it stalled? _(Optional : Only applied to Saving Entries)_
3. System validates the information provided
4. System fills the missing information, such as:
    - If no date, date is the current date
    - Status is DRAFT
    - Date of last change is the current date
    - Date of creation is the current date
5. System saves the new entry, providing the ID to the user 

### Alternative Flow (4) [User marked entry as not draft]

4.1. System fills the missing information, such as:
    - If no date, date is the current date
    - Status is PENDING _(If Multiple Payment or Saving : SCHEDULED case begin date is later)_ or DONE _(If Single Payment)_
    - Date of last change is the current date
    - Date of Creation of entry is the current date
    - Date of Finish of entry is the current date _(If Single Payment)_
4.2. _Back to step 5_

### Exception (3) [Information is invalid]

4. System informs to user that information provided is invalid
