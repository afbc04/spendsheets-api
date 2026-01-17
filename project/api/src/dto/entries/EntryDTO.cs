namespace DTO {

    // Exception when some proprieties break rules
    public class EntryDTOException : Exception {

        public string message {private set; get;}

        public EntryDTOException(string message) {
            this.message = message;
        }

    }

    public class EntryTransactionDTO : EntryDTO {

        public EntryTransactionDTO() : base(new EntryTransaction(0,true,0)) {}
        public EntryTransactionDTO(long ID) : base(new EntryTransaction(ID,true,0)) {}
        public EntryTransactionDTO(EntryTransaction entry) : base(entry) {}

        public override void set_date(DateOnly date) {

            if (date > this._entry.creation_date)
                throw new EntryDTOException($"Transaction entries can not have a future date");

            base.set_date(date);

        }

    }

    public class EntrySavingsDTO : EntryDTO {

        public EntrySavingsDTO() : base(new EntrySavings(0,true,0)) {}
        public EntrySavingsDTO(long ID) : base(new EntrySavings(ID,true,0)) {}
        public EntrySavingsDTO(EntrySavings entry) : base(entry) {}

        public override void set_money_amount(double money_amount) {

            if (money_amount < 0)
                throw new EntryDTOException("Savings Entry's target money must always be positive");

            base.set_money_amount(money_amount);

        }

        public override void set_money_spent(double? money_spent) {

            if (this._entry.status == EntryStatus.Done || this._entry.status == EntryStatus.Stalled)
                throw new EntryDTOException("Savings Entry's saved money can not exist when entry is stalled or done");

            if (money_spent != null) {

                if (money_spent < 0)
                    throw new EntryDTOException("Savings Entry's saved money must always be positive");

                if (Money.Convert32((double) money_spent) > this._entry.money)
                    throw new EntryDTOException("Savings Entry's saved money can not be higher than target money");

            }

            base.set_money_spent(money_spent);

        }

    }

    public class EntryCommitmentDTO : EntryDTO {

        public EntryCommitmentDTO() : base(new EntryCommitment(0,true,0)) {}
        public EntryCommitmentDTO(long ID) : base(new EntryCommitment(ID,true,0)) {}
        public EntryCommitmentDTO(EntryCommitment entry) : base(entry) {}

    }

    public abstract class EntryDTO {

        protected Entry _entry;

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
        public virtual void set_category(bool was_category_id_provided, Category? category) {

            if (category == null && was_category_id_provided == true)
                throw new EntryDTOException($"Category provided does not exists");

            this._entry.category_ID = category?.ID;

        }

        public virtual void set_monthly_service(bool was_monthly_service_id_provided, MonthlyService? monthly_service) {

            if (monthly_service == null && was_monthly_service_id_provided == true)
                throw new EntryDTOException($"Monthly Service provided does not exists");

            this._entry.monthly_service_ID = monthly_service?.ID;

        }
        
        public virtual void set_visible(bool is_visible) {
            this._entry.is_visible = is_visible;
        }

        public virtual void set_date(DateOnly date) {
            this._entry.date = date;
        }

        public virtual void set_due_date(DateOnly? due_date) {

            if (due_date != null && due_date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new EntryDTOException($"Due date should be a date from future");

            this._entry.due_date = due_date;

        }

        public virtual void set_description(string? description) {

            if (description != null && description.Length >= EntryRules.description_length_max)
                throw new EntryDTOException($"Description is too long (more than {EntryRules.description_length_max} characters)");

            this._entry.description = description;

        }

        public virtual void set_money_amount(double money_amount) {
            this._entry.money = Money.Convert32(money_amount);
        }

        public virtual void set_money_spent(double? money_spent) {
            this._entry.money_spent = money_spent != null ? Money.Convert32((double) money_spent) : null;
        }

        public virtual void set_status(string status) {

            try {
                this._entry.setStatus(status);
            }
            catch (EntryException ex) {
                throw new EntryDTOException(ex.message);
            }

        }

        public virtual void set_draft(bool is_draft) {

            if (is_draft)
                this._entry.setStatusDraft();
            else
                this._entry.undraftEntry();

        }

        public virtual void set_deleted(bool is_deleted) {

            if (is_deleted)
                this._entry.setStatusDeleted();
            else
                this._entry.recoverEntry();

        }

    }


}