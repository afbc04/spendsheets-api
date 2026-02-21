namespace DTO {

    public enum CollectionMapDTOFields {
        isMonthlyServiceActive
    }

    public class CollectionMapDTO {

        public bool[] fields_changed;

        public bool is_active { get; set; }

        public CollectionMapDTO() {
            this.fields_changed = [false];
            this.is_active = true;
        }

        public bool does_updates_anything() {
            return this.fields_changed.Any(s => s == true);
        }

        public void set_active(bool is_active) {
            this.is_active = is_active;
            this.fields_changed[(int) CollectionMapDTOFields.isMonthlyServiceActive] = true;
        }

    }

}