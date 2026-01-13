namespace DTO {

    // Exception when some proprieties break rules
    public class MonthlyServiceDTOException : Exception {

        public string message {private set; get;}

        public MonthlyServiceDTOException(string message) {
            this.message = message;
        }

    }

    public class MonthlyServiceDTO {

        private MonthlyServiceSimple _monthy_service;

        public MonthlyServiceDTO() {
            this._monthy_service = new MonthlyServiceSimple(0,"",true);
        }

        public MonthlyServiceDTO(long ID) {
            this._monthy_service = new MonthlyServiceSimple(ID,"",true);
        }

        public MonthlyServiceDTO(MonthlyServiceSimple monthly_service) {
            this._monthy_service = new MonthlyServiceSimple(monthly_service);
        }

        // Final method
        public MonthlyServiceSimple extract() {
            return this._monthy_service;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_name(string name) {

            if (name.Length >= MonthlyServiceRules.name_length_max)
                throw new MonthlyServiceDTOException($"Name is too long (more than {MonthlyServiceRules.name_length_max} characters)");

            this._monthy_service.name = name;

        }

        public void set_description(string? description) {

            if (description != null && description.Length >= MonthlyServiceRules.description_length_max)
                throw new MonthlyServiceDTOException($"Description is too long (more than {MonthlyServiceRules.description_length_max} characters)");

            this._monthy_service.description = description;

        }

        public void set_category_related(bool was_category_id_provided,Category? category) {

            if (category == null && was_category_id_provided == true)
                throw new MonthlyServiceDTOException($"Category provided does not exists");

            this._monthy_service.category_related = category?.ID;

        }

        public void set_money_amount(double? money_amount) {

            if (money_amount == null) {
                this._monthy_service.money_amount = null;
                return;
            }


            if (money_amount < 0)
                throw new MonthlyServiceDTOException("Initial money can not be negative");

            this._monthy_service.money_amount = Money.Convert32((double) money_amount);

        }

        public void set_active(bool is_active) {
            this._monthy_service.active = is_active;
        }

    }


}