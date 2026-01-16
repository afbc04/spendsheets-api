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

        public EntryTransactionDTO(EntryTransaction entry) {
            this._entry = new EntryTransaction(entry);
        }

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

        public void set_monthly_service(bool was_monthly_service_id_provided, MonthlyService? monthly_service) {

            if (monthly_service == null && was_monthly_service_id_provided == true)
                throw new EntryTransactionDTOException($"Monthly Service provided does not exists");

            this._entry.monthly_service_ID = monthly_service?.ID;

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

            try {
                this._entry.setStatus(status);
            }
            catch (EntryException ex) {
                throw new EntryTransactionDTOException(ex.message);
            }

        }

    }


}