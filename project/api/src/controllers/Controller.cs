using PacketHandlers;
using Nito.AsyncEx;
using Pages;
using Queries;

namespace Controller {

    public class ControllerManager {

        private readonly AsyncReaderWriterLock _lock;

        private ConfigController _config;

        public ControllerManager() {
            this._lock = new();
            this._config = new ConfigController();
        }

        /// #################################
        ///           CONFIG
        /// #################################
        public async Task<SendingPacket> config_create(IDictionary<string,object> request_data) {

            var controller_lock = await this._lock.WriterLockAsync();
            var config_manager_lock = await this._config.Lock.WriterLockAsync();
            controller_lock.Dispose();

            try {

                var response = await this._config.Create(request_data);
                return response;
                
            } finally {
                config_manager_lock.Dispose();
            }
        
        }

        public async Task<SendingPacket> config_update(IDictionary<string,object> request_data) {

            var controller_lock = await this._lock.WriterLockAsync();
            var config_manager_lock = await this._config.Lock.WriterLockAsync();
            controller_lock.Dispose();

            try {

                var response = await this._config.Update(request_data);
                return response;
                
            } finally {
                config_manager_lock.Dispose();
            }
        
        }

        public async Task<SendingPacket> config_get() {

            var controller_lock = await this._lock.ReaderLockAsync();
            var config_manager_lock = await this._config.Lock.ReaderLockAsync();
            controller_lock.Dispose();

            try {

                var response = await this._config.Get();
                return response;
                
            } finally {
                config_manager_lock.Dispose();
            }

        }

    }

}

