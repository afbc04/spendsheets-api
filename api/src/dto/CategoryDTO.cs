namespace DTO {

    // Exception when some proprieties break rules
    public class CategoryDTOException : Exception {

        public string message {private set; get;}

        public CategoryDTOException(string message) {
            this.message = message;
        }

    }

    public class CategoryDTO {

        private Category _category;

        public CategoryDTO() {
            this._category = new Category(0,"");
        }

        public CategoryDTO(long ID) {
            this._category = new Category(ID,"");
        }

        public CategoryDTO(Category Category) {
            this._category = Category;
        }

        // Final method
        public Category extract() {
            return this._category;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_name(string name) {

            if (name.Length >= CategoryRules.name_length_max)
                throw new CategoryDTOException($"Name is too long (more than {CategoryRules.name_length_max} characters)");

            this._category.name = name;

        }

        public void set_description(string? description) {

            if (description != null && description.Length >= CategoryRules.description_length_max)
                throw new CategoryDTOException($"Description is too long (more than {CategoryRules.description_length_max} characters)");

            this._category.description = description;

        }

    }


}