namespace DTO {

    // Exception when some proprieties break rules
    public class EntryMovementDTOException : Exception {

        public string message {private set; get;}

        public EntryMovementDTOException(string message) {
            this.message = message;
        }

    }

    public class EntryMovementDTO {

        private EntryMovement _entry_movement;

        public EntryMovementDTO() {
            this._entry_movement = new EntryMovement(0,0);
        }

        public EntryMovementDTO(long ID) {
            this._entry_movement = new EntryMovement(ID,0);
        }

        public EntryMovementDTO(EntryMovement entry_Movement) {
            this._entry_movement = new EntryMovement(entry_Movement);
        }

        // Final method
        public EntryMovement extract() {
            return this._entry_movement;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_money(double money) {
            this._entry_movement.money = Money.Convert32(money);
        }

        public void set_date(DateOnly date) {

            if (date > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new EntryDTOException($"Entries' movements can not have a future date");

            this._entry_movement.date = date;

        }

        public void set_comment(string? comment) {

            if (comment != null && comment.Length >= EntryRules.movement_comment_length_max)
                throw new EntryMovementDTOException($"Comment is too long (more than {EntryRules.movement_comment_length_max} characters)");

            this._entry_movement.comment = comment;

        }

    }


}