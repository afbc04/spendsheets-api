A - JOIN

= 0 -> 1788

B - Acc

= 0 -> 91ms

>= moneymount -> 152ms

Sort:
- ID
- Date
- Creation Date
- Finish Date
- Actual Money
- Target Money

Filter:
- TYPE
- Dates
- Date
- Creation Date
- Finish Date
- Status
- Money
- effective
- Category
- Collection


/statistics/general

```json
{
    "entries"
    "paymentEntries"
    "loanEntries"
    "categories"
    "collections"
    "monthlyServices"
    "monthlyServicesPercentage"
    "moneyTotalExpected"
    "moneyTotalReal"
}
```

/statistics/category/{ID}
/statistics/collection/{ID}
/statistics/


-- Transaction
```json
{
    "categoryId" : 1,
    "collectionId" : 2,
    "visible" : true,
    "public" : false,
    "type" : "payment",
    "active" : true,
    "date"
    "description"
    "actualMoney"
    "targetMoney" : null,
    "dueDate" : null,
    "status" : draft | done | deleted
}
```

-- Due payment
```json
{
    "categoryId" : 1,
    "collectionId" : 2,
    "visible" : true,
    "public" : false,
    "type" : "payment",
    "active" : true,
    "date"
    "description"
    "actualMoney"
    "targetMoney" 
    "dueDate" 
    "status" : draft | pending | ongoing | completed | stalled | done | deleted | ignored | cancelled
}
```

-- Loan
```json
{
    "categoryId" : 1,
    "collectionId" : 2,
    "visible" : true,
    "public" : false,
    "type" : "loan",
    "active" : true,
    "date"
    "description"
    "actualMoney"
    "targetMoney" 
    "dueDate" 
    "status" : draft | pending | ongoing | completed | stalled | done | deleted | ignored | cancelled
}
```


```json
{
    "id": 11,
    "category": {
        "id": 8,
        "name": "Fuel",
        "description": null
    },
    "collection": {
        "id": 11,
        "name": "b",
        "description": null
    },
    "visible": true,
    "public" : false,
    "type": "payment", | loan | savings
    "active" : true,
    "date": "2026-01-16",
    "description": "ekhjfbkejf",
    "targetMoney": 1,
    "actualMoney": -0.5,
    "remainingMoney": 1.5,
    "lastChangeDate": "2026-01-16 21:32:15",
    "creationDate": "2026-01-16",
    "finishDate": null,
    "dueDate": "2026-01-20",
    "draft": false,
    "finished": false,
    "completed": false,
    "pending": true,
    "deleted": false,
    "deletedDate": null,
    "status": "ongoing",
    "notes" : [
        {
            "id" : 1,
            "note" : "Payment #1",
            "date" : "",
            "money" : null
        }
    ],
    "_links": {
        "self": "/v1.0/entries/11",
        "notes": "/v1.0/entries/11/notes"
    }
}
```