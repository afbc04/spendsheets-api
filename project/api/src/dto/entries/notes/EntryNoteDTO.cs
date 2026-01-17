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

        public EntryNoteDTO() {
            this._entry_note = new EntryNote(0,"");
        }

        public EntryNoteDTO(long ID) {
            this._entry_note = new EntryNote(ID,"");
        }

        public EntryNoteDTO(EntryNote entry_note) {
            this._entry_note = new EntryNote(entry_note);
        }

        // Final method
        public EntryNote extract() {
            this._entry_note.changeDate = DateOnly.FromDateTime(DateTime.UtcNow);
            return this._entry_note;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_note(string note) {

            if (note.Length >= EntryRules.note_length_max)
                throw new EntryNoteDTOException($"Note is too long (more than {EntryRules.note_length_max} characters)");

            this._entry_note.note = note;

        }

    }


}