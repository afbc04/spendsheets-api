using Models;

namespace Pages {

    public class PageOutput {

        public long total_elements {get; private set;}
        public long page_elements {get; private set;}
        public long page {get; private set;}
        public long limit {get; private set;}
        public long total_pages {get; private set;}
        public bool empty {get; private set;}
        public bool all {get; private set;}
        public bool first_page {get; private set;}
        public bool last_page {get; private set;}
        public IList<IDictionary<string,object>> data {get; private set;}

        public PageOutput(PageInput page_input,long total_elements, IList<IDictionary<string,object>> data) {

            this.data = data;
            this.page = page_input.page;
            this.limit = page_input.limit;
            this.total_elements = total_elements;
            this.page_elements = data.Count();
            this.empty = this.page_elements == 0;
            this.all = this.page_elements == this.total_elements;

            this.total_pages = this.total_elements / page_input.limit;
            if (this.total_elements > this.total_pages * page_input.limit)
                this.total_pages++;

            this.first_page = this.page == 1;
            this.last_page = this.total_pages == 0 ? true : this.page == this.total_pages;

        }

        public IDictionary<string,object> to_json() {
            return new Dictionary<string,object> {
                ["totalElements"] = this.total_elements,
                ["pageElements"] = this.page_elements,
                ["page"] = this.page,
                ["limit"] = this.limit,
                ["totalPages"] = this.total_pages,
                ["empty"] = this.empty,
                ["all"] = this.all,
                ["firstPage"] = this.first_page,
                ["lastPage"] = this.last_page,
                ["data"] = this.data
            };
        }

    }

}