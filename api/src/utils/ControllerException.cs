namespace Controller {

    public class ControllerManagerException : Exception {

        public string error_message;

        public ControllerManagerException(string message) {
            this.error_message = message;
        }
      
    }

}

