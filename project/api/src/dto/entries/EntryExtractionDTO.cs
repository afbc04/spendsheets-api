namespace DTO {

    // Exception when some proprieties break rules
    public class EntryExtractionDTOException : Exception {

        public string message {private set; get;}

        public EntryExtractionDTOException(string message) {
            this.message = message;
        }

    }

    public class EntryExtractionDTO {

        private IDictionary<string,object> _data;
        private EntryType? _type;
        private bool should_provide_minimum_requirements;

        public EntryExtractionDTO(IDictionary<string,object> data, bool should_provide_minimum_requirements, EntryType? type) {
            
            this._type = type ?? EntryTypeHandler.Get((string) data["type"]!);
            this._data = data;
            this.should_provide_minimum_requirements = should_provide_minimum_requirements;

            if (this._type == null)
                throw new EntryExtractionDTOException($"Current type does not exists. Try [{string.Join(" | ", EntryTypeHandler.types)}]");

            else if (this._type == EntryType.Transaction)
                _HandleTransaction();

            else if (this._type == EntryType.Commitment)
                _HandleCommitment();

            else if (this._type == EntryType.Saving)
                _HandleSavings();

            else
                throw new EntryExtractionDTOException($"Current type does not exists. Try [{string.Join(" | ", EntryTypeHandler.types)}]");

            _ConvertStringToDate();

        }

        public IDictionary<string,object> getData() {
            return this._data;
        }

        public EntryType? getType() {
            return this._type;
        }

        // <i name="categoryId" datatype="integer" null="true" />
        // <i name="monthlyServiceId" datatype="integer" null="true" />
        // <i name="actualMoney" required="true" datatype="float" />
        // <i name="date" datatype="date" />
        // <i name="description" null="true" />
        // <i name="visible" datatype="boolean" />
        // <i name="status" /> 

        private void _HandleTransaction() {
            
            if (this._data.ContainsKey("actualMoney") == false && this.should_provide_minimum_requirements)
                throw new EntryExtractionDTOException("Transaction entries must have actual money. Its required to be send");

            if (this._data.ContainsKey("targetMoney"))
                throw new EntryExtractionDTOException("Transaction entries do not have target money. Do not send it");

            if (this._data.ContainsKey("dueDate"))
                throw new EntryExtractionDTOException("Transaction entries do not have due date. Do not send it");

        }

        // <i name="categoryId" datatype="integer" null="true" />
        // <i name="monthlyServiceId" datatype="integer" null="true" />
        // <i name="targetMoney" required="true" datatype="float" />
        // <i name="date" datatype="date" />
        // <i name="dueDate" null="true" datatype="date" />
        // <i name="description" null="true" />
        // <i name="visible" datatype="boolean" />
        // <i name="status" /> 

        private void _HandleCommitment() {
            
            if (this._data.ContainsKey("targetMoney") == false && this.should_provide_minimum_requirements)
                throw new EntryExtractionDTOException("Commitment entries must have target money. Its required to be send");
            
            if (this._data.ContainsKey("actualMoney"))
                throw new EntryExtractionDTOException("Commitment entries actual money is not obtain in entry's information. You should use movements to perform such effect");

        }

        // <i name="categoryId" datatype="integer" null="true" />
        // <i name="targetMoney" datatype="float" />
        // <i name="actualMoney" datatype="float" />
        // <i name="date" datatype="date" />
        // <i name="description" null="true" />
        // <i name="visible" datatype="boolean" />
        // <i name="status" /> 

        private void _HandleSavings() {

            if (this._data.ContainsKey("monthlyServiceId"))
                throw new EntryExtractionDTOException("Savings entries do not have monthly services. Do not send it");

        }
    
        private void _ConvertStringToDate() {

            if (this._data.ContainsKey("date"))
                this._data["date"] = DateOnly.Parse((string) this._data["date"]!);
        
            if (this._data.ContainsKey("dueDate") && this._data["dueDate"] != null)
                this._data["dueDate"] = DateOnly.Parse((string) this._data["dueDate"]!);
        
        }


    }

}