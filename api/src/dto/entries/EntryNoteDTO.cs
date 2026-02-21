namespace DTO {

    // Exception when some proprieties break rules
    public class EntryNoteDTOException : Exception {

        public string message {private set; get;}

        public EntryNoteDTOException(string message) {
            this.message = message;
        }

    }

    public class EntryNoteDTO {

        private EntryNote _entry_note;
        private int _money_delta;

        public EntryNoteDTO() {
            this._entry_note = new EntryNote(0);
            this._money_delta = 0;
        }

        public EntryNoteDTO(long ID) {
            this._entry_note = new EntryNote(ID);
            this._money_delta = 0;
        }

        public EntryNoteDTO(EntryNote entry_Note) {
            this._entry_note = new EntryNote(entry_Note);
            this._money_delta = 0;
        }

        // Final method
        public EntryNote extract() {

            if (this._entry_note.money == null && this._entry_note.note == null)
                throw new EntryNoteDTOException("Entry notes cannot be empty");

            return this._entry_note;
        }

        public int getMoneyDelta() {
            return this._money_delta;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_money(double? money) {

            int money_before = this._entry_note.money ?? 0;
            int money_after = Money.Convert32(money ?? 0);

            this._money_delta = money_after - money_before;
            this._entry_note.money = money != null ? money_after : null;
        }

        public void set_date(DateOnly date) {

            if (date > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new EntryDTOException($"Entries' notes can not have a future date");

            this._entry_note.date = date;

        }

        public void set_note(string? note) {

            if (note != null && note.Length >= EntryRules.note_length_max)
                throw new EntryNoteDTOException($"Note is too long (more than {EntryRules.note_length_max} characters)");

            this._entry_note.note = note;

        }

    }


}