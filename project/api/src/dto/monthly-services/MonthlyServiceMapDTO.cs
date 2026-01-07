namespace DTO {

    public enum MonthlyServiceMapDTOFields {
        moneyAmount,
        active
    }

    public class MonthlyServiceMapDTO {

        public bool[] fields_changed;

        public int? money_amount { get; set; }
        public bool is_active { get; set; }

        public MonthlyServiceMapDTO() {
            this.fields_changed = [false,false];
            this.money_amount = null;
            this.is_active = true;
        }

        public bool does_updates_anything() {
            return this.fields_changed.Any(s => s == true);
        }

        public void set_money_amount(double? money_amount) {

            if (money_amount == null)
                this.money_amount = null;

            else {
                if (money_amount < 0)
                    throw new MonthlyServiceDTOException("Initial money can not be negative");

                this.money_amount = Convert.ToInt32(Utils.convert_from_money((double) money_amount));
                
            }
            
            this.fields_changed[(int) MonthlyServiceMapDTOFields.moneyAmount] = true;

        }

        public void set_active(bool is_active) {
            this.is_active = is_active;
            this.fields_changed[(int) MonthlyServiceMapDTOFields.active] = true;
        }

    }


}