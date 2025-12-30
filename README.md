# Deploy

### Prerequisites
- Docker
- Docker Compose

### 1. Cloning Project

    git clone git@github.com:afbc04/spendsheets.git

    cd spendsheets/

### 2. Setup Project

    touch .env

Variables should have this format:

```.env
POSTGRES_DB=spendSheetDev
POSTGRES_USER=admin
POSTGRES_PASSWORD=admin
API_PORT=7777
```

### 3. Building

    docker compose build

### 4. Starting

    docker compose up -d

### 5. Cleaning

    docker compose down

### Extra Usage

    docker exec -it ss-database psql -U admin -d spendSheetDev

# Report

`INSERT SUMMARY HERE`

## 0. Index

## 1. Introduction

### 1.1. Contextualization

This project has the objective to be an useful and complete project made by me.  
Managing the usage of money it's important, therefore this project was born to help with it.

### 1.2. Motivation & objectives

The main problems with money management are:
- Keeping track of money borrowed
- Keeping track of unpaid loans or services
- Noticing where the money is spent, leading to realization some bad usage


To help with those problems, this project:
- Saves loans and it's movements, informing if they are finished and how much money is still required
- Saves money transitions, making a history of money spent or received
- With help of tags and categories, entries can be grouped up
- Display statistics related to these entries, helping the management of money usage

### Project Identity

### Development Plan

## 2. Requirements

### Funcional Requirements

#### 1. Config

**Configuration** informs how the system should behave, giving ways to obtain access tokens

| File | Description |
|------|--------------|
| [getting-token.md](use-cases/config/getting-token.md) | Obtain the access token |
| [change-to-reader-token.md](use-cases/config/change-to-reader-token.md) | Switches token's reader mode |
| [deleteting-token.md](use-cases/config/deleteting-token.md) | Delete the access token |
| [create-config.md](use-cases/config/create-config.md) | Configure the system |
| [config-details.md](use-cases/config/config-details.md) | View configuration details |
| [edit-config.md](use-cases/config/edit-config.md) | Edit configuration |
| [update-entries.md](use-cases/config/update-entries.md) | Update entries when system boots up |

#### 2. Classifiers

**Category** helps to split entries into different categories

| File | Description |
|------|------------|
| [add-category.md](use-cases/category/add-category.md) | Create a new category |
| [category-details.md](use-cases/category/category-details.md) | Get category details |
| [delete-category.md](use-cases/category/delete-category.md) | Delete an existing category |
| [edit-category.md](use-cases/category/edit-category.md) | Edit a category |
| [list-categories.md](use-cases/category/list-categories.md) | List all categories |

**Tag** helps to group entries

| File | Description |
|------|------------|
| [add-tag.md](use-cases/tags/add-tag.md) | Create a tag |
| [edit-tag.md](use-cases/tags/edit-tag.md) | Edit a tag |
| [delete-tag.md](use-cases/tags/delete-tag.md) | Delete a tag |
| [list-tags.md](use-cases/tags/list-tags.md) | List all tags |
| [tag-details.md](use-cases/tags/tag-details.md) | View tag details |

#### 3. Monthly Services

**Monthly Services** helps the system automatically generating entries

| File | Description |
|------|------------|
| [add-service.md](use-cases/monthly-service/add-service.md) | Add a monthly service |
| [edit-service.md](use-cases/monthly-service/edit-service.md) | Edit a monthly service |
| [delete-service.md](use-cases/monthly-service/delete-service.md) | Delete a monthly service |
| [list-services.md](use-cases/monthly-service/list-services.md) | List monthly services |
| [service-details.md](use-cases/monthly-service/service-details.md) | View monthly service details |

#### 4. Statistics

**Statistics** provides informations about loads of entities of the system

_Loads of statistics will be dealt soon_

| File | Description |
|------|------------|
| [general-infos.md](use-cases/statistic/general-infos.md) | General statistics |
| [entries-infos.md](use-cases/statistic/entries-infos.md) | Entry-related statistics |
| [money-infos.md](use-cases/statistic/money-infos.md) | Financial statistics |

#### 5. Entries

**Entries** are the main protagonist, being all different entries treated the same way

| File | Description |
|------|------------|
| [create-entry.md](use-cases/entries/create-entry.md) | Create a new entry |
| [edit-entry.md](use-cases/entries/edit-entry.md) | Edit an entry |
| [delete-entry.md](use-cases/entries/delete-entry.md) | Delete an entry |
| [entry-details.md](use-cases/entries/entry-details.md) | View entry details |
| [list-entries.md](use-cases/entries/list-entries.md) | List all entries |
| [finish-entry.md](use-cases/entries/finish-entry.md) | Mark an entry as finished |
| [recover-entry-from-trash.md](use-cases/entries/recover-entry-from-trash.md) | Restore deleted entry |
| [changing-saving-status.md](use-cases/entries/changing-saving-status.md) | Change saving status |
| [undrafting-a-draft.md](use-cases/entries/undrafting-a-draft.md) | Convert draft to active entry |
| [add-line.md](use-cases/entries/add-line.md) | Add a line to an entry |
| [edit-line.md](use-cases/entries/edit-line.md) | Edit an entry line |
| [delete-line.md](use-cases/entries/delete-line.md) | Delete an entry line |
| [add-note.md](use-cases/entries/add-note.md) | Add a note to an entry |
| [edit-note.md](use-cases/entries/edit-note.md) | Edit an entry note |
| [remove-note.md](use-cases/entries/remove-note.md) | Remove an entry note |
| [add-tag.md](use-cases/entries/add-tag.md) | Add a tag to an entry |
| [delete-tag.md](use-cases/entries/delete-tag.md) | Remove a tag from an entry |

### Non-Functional Requirements

The system shall:
- Efficiently handle a high volume of read operations
- Support write operations performed by a single user, without requiring concurrency handling
- Securely store user passwords using encryption
- Be flexible and maintainable, allowing adaptation to future updates or changes.
- Operate primarily in offline mode, becoming online only when explicitly activated by the system owner
- Scale to support a large number of entries without performance degradation
- Provide acceptable response times for all supported operations

## 3. System Design

## 4. Database

### Schematics

**Money** will be treated as a int, where the first 2 digits represent the cents, since there isnt 0.005. _Example: 4624 -> 46.24€_

---

#### Config

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **id** | Unique ID of config _(Has no impact)_ | int | `1` | No |
| **databaseVersion** | Version of the database | int | `1` | No |
| **lastOnlineDate** | Date of last time system was online | datetime | `"2025-12-25 17:46:52"` | No |
| **username** | Username of User of system | VARCHAR(30) | `"afbc"` | Yes |
| **password** | Password of User of system | STRING _(hashed)_ | `"djvfejvfje"` | No | 
| **passwordSalt** | Salt of password of User of system | int | `4324353` | No |
| **nameOfUser** | Name of User of system | VARCHAR(64) | `"André"` | Yes |
| **isPublic** | Is the system public? | BOOLEAN | `false` | No |
| **initMoney**| Initial money of User | int | `13400` | No |
| **lostMoney**| Lost money of User | int | `1000` | No |
| **savedMoney**| Saved money of User | int | `2700` | No |

---

#### Categories

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **id** | Unique Identifier of Category | int | `1` | No |
| **name** | Name of category | VARCHAR(50) | `"Fuel"` | No |
| **description** | Description of category | VARCHAR(256) | `"Gasoline spent on car"` | Yes | 

---

#### Tags

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **id** | Unique Identifier of Tag | int | `1` | No |
| **name** | Name of tag | VARCHAR(50) | `"xmas2025"` | No |
| **description** | Description of tag | VARCHAR(256) | `"Entries related to Christmas of 2025"` | Yes | 

---

#### Monthly Services

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **id** | Unique Identifier of Monthly Service | int | `1` | No |
| **name** | Name of monthly service | VARCHAR(50) | `"eletricity"` | No |
| **description** | Description of monthly service | VARCHAR(256) | `"Bills related to Eletricity"` | Yes | 
| **categoryRelated** | ID of category related to this monthly service | int | `1` | Yes | 
| **moneyAmount** | Current money amount of this monthly service | int | `12500` | Yes | 
| **isActive** | Is this monthly service active? | BOOLEAN | `true` | No | 

---

#### Entries

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **id** | Unique Identifier of Entry | int | `1` | No |
| **categoryId** | ID of category related to this entry | int | `3` | Yes |
| **isVisible** | Is this entry visible? | BOOLEAN | `true` | No | 
| **type** | Type of Entry | int | `1` | No |
| **moneyAmount** | Money added or removed when entry is finished | int | `1750` | No |
| **lastChangeDate** | Date of last time entry was updated | datetime | `"2025-12-25 17:46:52"` | No |
| **creationDate** | Date when entry was created | date | `"2025-12-25"` | No |
| **finishDate** | Date when entry was finished | date | `"2025-12-25"` | Yes |
| **description** | Description of entry | VARCHAR(128) | `"Spent in fuel"` | Yes |
| **status** | Status of Entry | int | `2` | No |

Notes:
- **type:**
    - 0 : _Transation_
    - 1 : _Commitment_
    - 2 : _Saving_
- **status:**
    - 0 : _Draft_
    - 1 : _Started_
    - 2 : _Done_
    - 3 : _Deleted_
    - 4 : _Stalled_
    - 5 : _Accomplished_

---

#### Deleted Entries

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **id** | ID of deleted entry | int | `1` | No |
| **deletionDate** | Date when entry was deleted/cancelled/ignored | date | `"2025-12-25"` | No |
| **deletedStatus** | Status of deleted entry | int | `0` | No |
| **lastStatus** | Previous status of deleted entry | int | `2` | No |

- **deletedStatus:**
    - 0 : _Deleted_
    - 1 : _Cancelled_
    - 2 : _Ignored_

---

#### Commitment Entries

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **id** | ID of entry | int | `1` | No |
| **monthlyServiceId** | ID of monthly service related to this entry | int | `3` | Yes |
| **isGenerated** | Is this entry generated by system? | BOOLEAN | `true` | No | 
| **beginDate** | Date when entry is gonna be considered | date | `"2025-12-25"` | Yes |
| **scheduleDueDate** | Date when entry is planned to end | date | `"2025-12-25"` | Yes |
| **realDueDate** | Date when entry was ended | date | `"2025-12-25"` | Yes |
| **moneyAmountLeft** | Money left in this entry | int | `1750` | No |

---

#### Tags of Entries

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **entryId** | ID of entry | int | `1` | No |
| **tagId** | ID of tag | int | `3` | No |

---

#### Notes of Entries

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **entryId** | ID of entry | int | `1` | No |
| **noteId** | ID of note | int | `1` | No |
| **note** | Text of note | VARCHAR(64) | `"Fuel Simple Gasoline 95"` | No |
| **date** | Date when note was added | date | `"2025-12-25"` | No |

---

#### Movements of Entries

| Name of Attribute | Description | Datatype | Examples | Nullable? |
|-------------------|-------------|-----------|-----------|
| **entryId** | ID of entry | int | `1` | No |
| **movementId** | ID of movement | int | `1` | No |
| **money** | Amount of money related to this movement | int | `500` | No |
| **comment** | Text of movements | VARCHAR(64) | `"Gift"` | Yes |
| **date** | Date when movement was added | date | `"2025-12-25"` | No |

---

## 5. Business Logic

Config

```json
{
    "name" : "André",
    "username" : "afbc",
    "public" : false,
    "lastOnlineDate" : "2025-12-25 17:46:52",
    "tokenValid" : false,
    "writer" : true
}
```

Category

```json
{
    "id" : 1,
    "name" : "Fuel",
    "description" : "Gasoline spent on car"
}
```

Tags

```json
{
    "id" : 1,
    "name" : "xmas2025",
    "description" : "Entries related to Christmas of 2025"
}
```

Monthly Services

```json
{
    "id" : 1,
    "name" : "eletricity",
    "description" : "Bills related to Eletricity",
    "categoryRelated" : {
        "id" : 1,
        "name" : "Fuel",
        "description" : "Gasoline spent on car"
    },
    "moneyAmount" : 125.0,
    "active" : true
}
```

Entries - Transaction Max

```json
{
    "id" : 1,
    "category" : {
        "id" : 1,
        "name" : "Fuel",
        "description" : "Gasoline spent on car"
    },
    "notes" : [
        {
            "id" : 1,
            "note" : "Note 1",
            "date" : "2025-12-25"
        }
    ],
    "tags" : [
        {
            "id" : 17,
            "name" : "car",
            "description" : "Entries related to car"
        },
        {
            "id" : 1,
            "name" : "xmas2025",
            "description" : "Entries related to Christmas of 2025"
        }
    ],
    "visible" : true,
    "type" : "transaction",
    "money" : -50.0,
    "lastChangeDate" : "2025-12-25 17:46:52",
    "creationDate" : "2025-12-25",
    "finishDate" : "2025-12-25",
    "description" : "Spent in fuel",
    "draft" : false,
    "finished" : true,
    "deleted" : false,
    "generatedBySystem" : false,
    "status" : "done"
}
```

Entries - Transaction Min

```json
{
    "id" : 1,
    "category" : null,
    "notes" : [],
    "tags" : [],
    "visible" : true,
    "type" : "transaction",
    "money" : -50.0,
    "lastChangeDate" : "2025-12-25 17:46:52",
    "creationDate" : "2025-12-25",
    "finishDate" : null,
    "description" : null,
    "draft" : true,
    "finished" : false,
    "deleted" : false,
    "generatedBySystem" : false,
    "status" : "draft"
}
```

Entries - Saving Max

```json
{
    "id" : 1,
    "category" : {
        "id" : 1,
        "name" : "Fuel",
        "description" : "Gasoline spent on car"
    },
    "notes" : [
        {
            "id" : 1,
            "note" : "Note 1",
            "date" : "2025-12-25"
        }
    ],
    "tags" : [
        {
            "id" : 17,
            "name" : "car",
            "description" : "Entries related to car"
        },
        {
            "id" : 1,
            "name" : "xmas2025",
            "description" : "Entries related to Christmas of 2025"
        }
    ],
    "visible" : true,
    "type" : "saving",
    "money" : 0,
    "moneySaving" : 350.0,
    "lastChangeDate" : "2025-12-25 17:46:52",
    "creationDate" : "2025-12-25",
    "finishDate" : "2025-12-25",
    "description" : "Spent in fuel",
    "draft" : false,
    "finished" : true,
    "stalled" : true,
    "accomplished" : false,
    "deleted" : false,
    "generatedBySystem" : false,
    "status" : "accomplished"
}
```

Entries - Saving Min

```json
{
    "id" : 1,
    "category" : null,
    "notes" : [],
    "tags" : [],
    "visible" : true,
    "type" : "saving",
    "money" : 0,
    "moneySaving" : 350.0,
    "lastChangeDate" : "2025-12-25 17:46:52",
    "creationDate" : "2025-12-25",
    "finishDate" : null,
    "description" : null,
    "draft" : false,
    "finished" : false,
    "stalled" : true,
    "accomplished" : false,
    "deleted" : false,
    "generatedBySystem" : false,
    "status" : "stalled"
}
```

Entries - Commitment Max

```json
{
    "id" : 1,
    "category" : {
        "id" : 1,
        "name" : "Eletricity",
        "description" : "Lights"
    },
    "notes" : [
        {
            "id" : 1,
            "note" : "Note 1",
            "date" : "2025-12-25"
        }
    ],
    "tags" : [
        {
            "id" : 13,
            "name" : "lights",
            "description" : "Entries related to lights"
        },
        {
            "id" : 1,
            "name" : "xmas2025",
            "description" : "Entries related to Christmas of 2025"
        }
    ],
    "visible" : true,
    "monthlyService" : {
        "id" : 1,
        "name" : "eletricity",
        "description" : "Bills related to Eletricity",
        "active" : true
    },
    "type" : "commitment",
    "money" : -120.0,
    "moneyLeft" : 0,
    "lastChangeDate" : "2025-12-25 17:46:52",
    "creationDate" : "2025-12-25",
    "finishDate" : "2025-12-25",
    "description" : "Spent in fuel",
    "beginDate" : "2025-12-25",
    "scheduleDueDate" : "2026-01-25",
    "realDueDate" : "2025-12-25",
    "movements" : [
        {
            "id" : 1,
            "money" : -60.0,
            "date" : "2025-12-25",
            "comment" : "1st payment"
        },
        {
            "id" : 2,
            "money" : -60.0,
            "date" : "2025-12-25",
            "comment" : "2nd payment"
        }
    ],
    "draft" : false,
    "finished" : true,
    "deleted" : false,
    "generatedBySystem" : false,
    "status" : "done"
}
```

## 6. User Interfaces

## 7. Usage

## 8. Testing

## 9. Conclusion