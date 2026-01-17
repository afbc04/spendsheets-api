using Npgsql;

namespace DAO {

    public static class EntryGetListDAO {

        public static EntryList serialize_list(NpgsqlDataReader r) {

            var category = _serialize_list_category_of_entry(r);
            var monthly_service = _serialize_list_monthly_service_of_entry(r);

            return new EntryList(
                r.getLong((int) EntryListFields.id),
                category,
                monthly_service,
                EntryTypeHandler.exportDAO(r.getChar((int) EntryListFields.type)),
                r.getInt((int) EntryListFields.money_amount),
                r.tryGetInt((int) EntryListFields.money_amount_spent),
                r.getDate((int) EntryListFields.date),
                r.tryGetDate((int) EntryListFields.due_date),
                EntryStatusHandler.exportDAO(r.getChar((int) EntryListFields.status)),
                r.IsDBNull((int) EntryListFields.deleted_status) == false ? EntryStatusHandler.exportDAODeleted(r.getChar((int) EntryListFields.deleted_status)) : null
            );
        }

        private static Category? _serialize_list_category_of_entry(NpgsqlDataReader r) {

            Category? category = null;
            long? category_id = r.tryGetLong((int) EntryListFields.category_id);
            if (category_id != null)
                category = new Category(
                    (long) category_id,
                    r.getString((int) EntryListFields.category_name)
                );

            return category;

        }

        private static MonthlyServiceSimple? _serialize_list_monthly_service_of_entry(NpgsqlDataReader r) {

            MonthlyServiceSimple? monthly_service = null;
            long? monthly_service_id = r.tryGetLong((int) EntryListFields.monthly_service_id);
            if (monthly_service_id != null)
                monthly_service = new MonthlyServiceSimple(
                    (long) monthly_service_id,
                    r.getString((int) EntryListFields.monthly_service_name),
                    true
                );

            return monthly_service;

        }

    }
}
