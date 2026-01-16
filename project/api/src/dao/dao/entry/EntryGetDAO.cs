using Npgsql;

namespace DAO {

    public static class EntryGetDAO {

        public static EntryTransaction serialize_transaction(NpgsqlDataReader r) {

            var deleted_entry_state = _serialize_deleted_entry(r);

            return new EntryTransaction(
                r.getLong((int) EntryDetailsFields.id),
                r.tryGetLong((int) EntryDetailsFields.category_id),
                r.tryGetLong((int) EntryDetailsFields.monthly_service_id),
                r.getBool((int) EntryDetailsFields.is_visible),
                r.getInt((int) EntryDetailsFields.money_amount),
                r.getDate((int) EntryDetailsFields.date),
                r.getDateTime((int) EntryDetailsFields.last_change_date),
                r.getDate((int) EntryDetailsFields.creation_date),
                r.tryGetDate((int) EntryDetailsFields.finish_date),
                r.tryGetString((int) EntryDetailsFields.description),
                EntryStatusHandler.exportDAO(r.getChar((int) EntryDetailsFields.status)),
                deleted_entry_state
            );
        }

        public static EntryDetails serialize_detailed(NpgsqlDataReader r) {

            var category = _serialize_category_of_entry(r);
            var monthly_service = _serialize_monthly_service_of_entry(r);
            var deleted_entry_state = _serialize_deleted_entry(r);

            return new EntryDetails(
                r.getLong((int) EntryDetailsFields.id),
                category,
                monthly_service,
                r.getBool((int) EntryDetailsFields.is_visible),
                EntryTypeHandler.exportDAO(r.getChar((int) EntryDetailsFields.type)),
                r.getInt((int) EntryDetailsFields.money_amount),
                r.tryGetInt((int) EntryDetailsFields.money_amount_spent),
                r.getDate((int) EntryDetailsFields.date),
                r.getDateTime((int) EntryDetailsFields.last_change_date),
                r.getDate((int) EntryDetailsFields.creation_date),
                r.tryGetDate((int) EntryDetailsFields.finish_date),
                r.tryGetDate((int) EntryDetailsFields.due_date),
                r.tryGetString((int) EntryDetailsFields.description),
                EntryStatusHandler.exportDAO(r.getChar((int) EntryDetailsFields.status)),
                deleted_entry_state?.deleted_date,
                deleted_entry_state?.delete_status,
                deleted_entry_state?.last_status
            );
        }

        private static DeletedEntryState? _serialize_deleted_entry(NpgsqlDataReader r) {

            DeletedEntryState? deleted_entry_state = null;
            DateOnly? deletion_date = r.tryGetDate((int) EntryDetailsFields.deletion_date);
            if (deletion_date != null)
                deleted_entry_state = new DeletedEntryState(
                    (DateOnly) deletion_date,
                    EntryStatusHandler.exportDAO(r.getChar((int) EntryDetailsFields.last_status)),
                    EntryStatusHandler.exportDAODeleted(r.getChar((int) EntryDetailsFields.deleted_status))
                );

            return deleted_entry_state;

        }

        private static Category? _serialize_category_of_entry(NpgsqlDataReader r) {

            Category? category = null;
            long? category_id = r.tryGetLong((int) EntryDetailsFields.category_id);
            if (category_id != null)
                category = new Category(
                    (long) category_id,
                    r.getString((int) EntryDetailsFields.category_name),
                    r.tryGetString((int) EntryDetailsFields.category_description)
                );

            return category;

        }

        private static MonthlyServiceSimple? _serialize_monthly_service_of_entry(NpgsqlDataReader r) {

            MonthlyServiceSimple? monthly_service = null;
            long? monthly_service_id = r.tryGetLong((int) EntryDetailsFields.monthly_service_id);
            if (monthly_service_id != null)
                monthly_service = new MonthlyServiceSimple(
                    (long) monthly_service_id,
                    r.getString((int) EntryDetailsFields.monthly_service_name),
                    r.tryGetString((int) EntryDetailsFields.monthly_service_description),
                    null,
                    r.getBool((int) EntryDetailsFields.monthly_service_active),
                    null
                );

            return monthly_service;

        }

    }
}
