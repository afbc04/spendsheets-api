namespace DTO {

    // Exception when some proprieties break rules
    public class EntryTransactionDTOException : Exception {

        public string message {private set; get;}

        public EntryTransactionDTOException(string message) {
            this.message = message;
        }

    }

    public class EntryTransactionDTO {

        private EntryTransaction _entry;

        public EntryTransactionDTO() {
            this._entry = new EntryTransaction(0,true,0);
        }

        public EntryTransactionDTO(long ID) {
            this._entry = new EntryTransaction(ID,true,0);
        }

        /*
        public EntryTransactionDTO(EntryTransaction entry) {
            this._entry = new EntryTransaction(entry);
        }*/

        // Final method
        public EntryTransaction extract() {
            this._entry.last_change_date = DateTime.UtcNow;
            return this._entry;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_category(bool was_category_id_provided, Category? category) {

            if (category == null && was_category_id_provided == true)
                throw new EntryTransactionDTOException($"Category provided does not exists");

            this._entry.category_ID = category?.ID;

        }
        
        public void set_visible(bool is_visible) {
            this._entry.is_visible = is_visible;
        }

        public void set_date(DateOnly date) {

            if (date > this._entry.creation_date)
                throw new EntryTransactionDTOException($"Transaction entries can not have a future date");

            this._entry.date = date;

        }

        public void set_description(string? description) {

            if (description != null && description.Length >= EntryRules.description_length_max)
                throw new EntryTransactionDTOException($"Description is too long (more than {EntryRules.description_length_max} characters)");

            this._entry.description = description;

        }

        public void set_money_amount(double money_amount) {
            this._entry.money = Money.Convert32(money_amount);
        }

        public void set_status(string status) {

            status = status.ToLower();

            switch (status) {
                case "draft":
                    this._entry.setStatusDraft();
                    break;
                case "done":
                    this._entry.setStatusDone();
                    break;
                case "deleted":
                    this._entry.setStatusDeleted();
                    break;
                default:
                    throw new EntryTransactionDTOException("Status provided is not valid in Transaction Entries");
            }

        }

    }


}