namespace DTO {

    // Exception when some proprieties break rules
    public class CollectionDTOException : Exception {

        public string message {private set; get;}

        public CollectionDTOException(string message) {
            this.message = message;
        }

    }

    public class CollectionDTO {

        private Collection _collection;

        public CollectionDTO() {
            this._collection = new Collection(0,"");
        }

        public CollectionDTO(long ID) {
            this._collection = new Collection(ID,"");
        }

        public CollectionDTO(Collection collection) {
            this._collection = new Collection(collection);
        }

        // Final method
        public Collection extract(bool is_monthly_service) {

            if (is_monthly_service) {
                this._collection.is_monthly_service_active = this._collection.is_monthly_service_active ?? true;
            }
            else {
                this._collection.category_ID = null;
                this._collection.is_monthly_service_active = null;
                this._collection.money_amount = null;
            }

            this._collection.is_monthly_service = is_monthly_service;

            return this._collection;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_name(string name) {

            if (name.Length >= CollectionRules.name_length_max)
                throw new CollectionDTOException($"Name is too long (more than {CollectionRules.name_length_max} characters)");

            this._collection.name = name;

        }

        public void set_description(string? description) {

            if (description != null && description.Length >= CollectionRules.description_length_max)
                throw new CollectionDTOException($"Description is too long (more than {CollectionRules.description_length_max} characters)");

            this._collection.description = description;

        }

        public void set_category(bool was_category_id_provided, Category? category) {

            if (category == null && was_category_id_provided == true)
                throw new CollectionDTOException($"Category provided does not exists");

            this._collection.category_ID = category?.ID;

        }

        public void set_money_amount(double? money_amount) {

            if (money_amount == null) {
                this._collection.money_amount = null;
                return;
            }


            if (money_amount < 0)
                throw new CollectionDTOException("Money amount can not be negative");

            this._collection.money_amount = Money.Convert32((double) money_amount);

        }

        public void set_monthly_service_active(bool is_monthly_service_active) {
            this._collection.is_monthly_service_active = is_monthly_service_active;
        }

    }


}