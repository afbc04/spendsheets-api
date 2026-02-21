public static class EntryStatusHandler {

    public static EntryStatus? Extract(string status) {

        return status.ToLower() switch {
            "draft" => EntryStatus.Draft,
            "ongoing" => EntryStatus.OnGoing,
            "done" => EntryStatus.Done,
            "stalled" => EntryStatus.Stalled,
            "deleted" => EntryStatus.Deleted,
            "cancelled" => EntryStatus.Cancelled,
            "ignored" => EntryStatus.Ignored,
            _ => null
        };

    }

    public static bool IsOngoing(EntryStatus status) =>
        status switch {
            EntryStatus.Pending => true,
            EntryStatus.OnGoing => true,
            EntryStatus.Completed => true,
            _ => false
        };

    public static bool IsDeleted(EntryStatus status) =>
        status switch {
            EntryStatus.Deleted => true,
            EntryStatus.Cancelled => true,
            EntryStatus.Ignored => true,
            _ => false
        };

    public static string Get(EntryList entry) {

        string status = entry.status switch {
            EntryStatus.Draft => "draft",
            EntryStatus.Pending => "pending",
            EntryStatus.OnGoing => "ongoing",
            EntryStatus.Completed => "completed",
            EntryStatus.Done => "done",
            EntryStatus.Deleted => "deleted",
            EntryStatus.Cancelled => "cancelled",
            EntryStatus.Ignored => "ignored",
            EntryStatus.Stalled => "stalled",
            _ => "???"
        };

        if (IsOngoing(entry.status)) {

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (entry.date > today)
                status = "scheduled";

            if (entry.due_date != null && entry.due_date < today)
                status = "overdue";

        }

        return status;

    }

    public static string Get(Entry entry) {

        string status = entry.status switch {
            EntryStatus.Draft => "draft",
            EntryStatus.Pending => "pending",
            EntryStatus.OnGoing => "ongoing",
            EntryStatus.Completed => "completed",
            EntryStatus.Done => "done",
            EntryStatus.Deleted => "deleted",
            EntryStatus.Cancelled => "cancelled",
            EntryStatus.Ignored => "ignored",
            EntryStatus.Stalled => "stalled",
            _ => "???"
        };

        if (IsOngoing(entry.status)) {

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (entry.date > today)
                status = "scheduled";

            if (entry.due_date != null && entry.due_date < today)
                status = "overdue";

        }

        return status;

    }

    public static EntryStatus exportDAO(char status) {
        return (EntryStatus) (status - '0');
    }

    public static char importDAO(EntryStatus status) {
        return (char) ('0' + ((int) status));
    }

}