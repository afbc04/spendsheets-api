public static class EntryStatusHandler {

    public static EntryStatus? Extract(string status) {

        return status.ToLower() switch {
            "draft" => EntryStatus.Draft,
            "ongoing" => EntryStatus.OnGoing,
            "done" => EntryStatus.Done,
            "stalled" => EntryStatus.Stalled,
            "accomplished" => EntryStatus.Accomplished,
            "deleted" => EntryStatus.Deleted,
            "cancelled" => EntryStatus.Deleted,
            "ignored" => EntryStatus.Deleted,
            _ => null
        };

    }

    public static DeletedEntryStatus? ExtractDelete(string status) {

        return status.ToLower() switch {
            "deleted" => DeletedEntryStatus.Deleted,
            "cancelled" => DeletedEntryStatus.Cancelled,
            "ignored" => DeletedEntryStatus.Ignored,
            _ => null
        };

    }

    public static string Get(DateOnly date, DateOnly? scheduled_date, EntryStatus status, DeletedEntryStatus? deleted_status, int money, int? money_spent) {

        if (status == EntryStatus.OnGoing) {

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (date > today)
                return "scheduled";

            if (scheduled_date != null && scheduled_date < today)
                return "overdue";

            if (money_spent != null) {

                if (money_spent == money)
                    return "completed";

                if (money_spent == 0)
                    return "pending";

            }
            
            return "ongoing";

        }
        else if (status == EntryStatus.Deleted) {

            return deleted_status == null ? "deleted" : deleted_status switch {
                DeletedEntryStatus.Cancelled => "cancelled",
                DeletedEntryStatus.Ignored => "ignored",
                _ => "deleted"
            };

        }
        else
            return status switch {
                EntryStatus.Draft => "draft",
                EntryStatus.Done => "done",
                EntryStatus.Stalled => "stalled",
                EntryStatus.Accomplished => "accomplished",
                _ => "corrupted"
            };

    }

    public static EntryStatus exportDAO(char status) {
        return (EntryStatus) (status - '0');
    }

    public static char importDAO(EntryStatus status) {
        return (char) ('0' + ((int) status));
    }

    public static DeletedEntryStatus exportDAODeleted(char status) {
        return (DeletedEntryStatus) (status - '0');
    }

    public static char importDAODeleted(DeletedEntryStatus status) {
        return (char) ('0' + ((int) status));
    }

}