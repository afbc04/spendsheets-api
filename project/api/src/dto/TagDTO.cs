namespace DTO {

    // Exception when some proprieties break rules
    public class TagDTOException : Exception {

        public string message {private set; get;}

        public TagDTOException(string message) {
            this.message = message;
        }

    }

    public class TagDTO {

        private Tag _tag;

        public TagDTO() {
            this._tag = new Tag(0,"");
        }

        public TagDTO(Tag tag) {
            this._tag = tag;
        }

        // Final method
        public Tag extract() {
            return this._tag;
        }

        // @@@@@@@@@@@@@@@@
        //    SETTERS
        // @@@@@@@@@@@@@@@@
        public void set_name(string name) {

            if (name.Length >= TagRules.name_length_max)
                throw new TagDTOException($"Name is too long (more than {TagRules.name_length_max} characters)");

            this._tag.name = name;

        }

        public void set_description(string? description) {

            if (description != null && description.Length >= TagRules.description_length_max)
                throw new TagDTOException($"Description is too long (more than {TagRules.description_length_max} characters)");

            this._tag.description = description;

        }

    }


}