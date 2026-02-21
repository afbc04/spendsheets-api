namespace DTO {

    // Exception when some proprieties break rules
    public class EntryDTOException : Exception {

        public string message {private set; get;}

        public EntryDTOException(string message) {
            this.message = message;
        }

    }

    public class EntryDTO {

        private Entry _entry;

        public EntryDTO() {
            this._entry = new Entry(0,EntryType.Payment);
        }

        public EntryDTO(long ID) {
            this._entry = new Entry(ID,EntryType.Payment);
        }

        public EntryDTO(Entry entry) {
            this._entry = entry;
        }

        // Final method
        public Entry extract() {
            this._entry.last_change_date = DateTime.UtcNow;
            return this._entry;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_category(bool was_category_id_provided, Category? category) {

            if (category == null && was_category_id_provided == true)
                throw new EntryDTOException($"Category provided does not exists");

            this._entry.category_ID = category?.ID;

        }

        public void set_collection(bool was_collection_id_provided, Collection? collection) {

            if (collection == null && was_collection_id_provided == true)
                throw new EntryDTOException($"Collection provided does not exists");

            this._entry.collection_ID = collection?.ID;

        }
        
        public void set_visible(bool is_visible) {
            this._entry.is_visible = is_visible;
        }

        public void set_public(bool is_public) {
            this._entry.is_public = is_public;
        }

        public void set_active(bool is_active) {
            this._entry.active = is_active;
        }

        public void set_type(string type) {

            EntryType? entry_type = EntryTypeHandler.Get(type);
            if (entry_type == null)
                throw new EntryDTOException($"Entry type is not valid. Types available [{ string.Join(",",EntryTypeHandler.types) }]");

            this._entry.type = (EntryType) entry_type;
            
        }

        public void set_money_target(double? money_target) {

            // No target money
            if (money_target == null) {
                this._entry.money_spent = null;
                this._entry.money = 0;
            }
            // Has target money
            else {
                this._entry.money_spent = 0;
                this._entry.money = Money.Convert32((double) money_target);
            }

        }

        // Pre-requisite : Money target should be set
        public void set_money_amount(double money_amount) {

            // No target money
            if (this._entry.money_spent == null) {
                this._entry.money = Money.Convert32(money_amount);
            }
            // Has target money
            else
                this._entry.money_spent = Money.Convert32(money_amount);

        }

        // Pre-requisite : Money target should be set
        public void set_date(DateOnly date) {

            // If has no target money, its talking about something in the past or that havent happened yet
            if (this._entry.money_spent == null && date > this._entry.creation_date)
                throw new EntryDTOException($"Entries with no target money can not have a future date");

            this._entry.date = date;

        }

        // Pre-requisite : Date should be set
        public void set_due_date(DateOnly? due_date) {

            if (due_date != null && due_date < this._entry.date)
                throw new EntryDTOException($"Due date should be a date from future");

            this._entry.due_date = due_date;

        }

        public void set_description(string? description) {

            if (description != null && description.Length >= EntryRules.description_length_max)
                throw new EntryDTOException($"Description is too long (more than {EntryRules.description_length_max} characters)");

            this._entry.description = description;

        }

        public void set_status(string status) {

            try {
                this._entry.setStatus(status);
            }
            catch (EntryException ex) {
                throw new EntryDTOException(ex.message);
            }

        }

        public void set_draft(bool is_draft) {

            if (is_draft)
                this._entry.setStatusDraft();
            else
                this._entry.undraftEntry();

        }

        public void set_deleted(bool is_deleted) {

            if (is_deleted)
                this._entry.setStatusDeleted();
            else
                this._entry.recoverEntry();

        }

    }

}