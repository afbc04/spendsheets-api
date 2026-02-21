using Serilog;

namespace DAO {

    public static class DAOUpdate {

        public static bool Upgrade(long system_database_version) {

            long N = DAOManager.DatabaseVersion - system_database_version;

            try {

                for (int i = 1; i <= N ; i++) {

                    long version_to_be_updated = system_database_version + i;
                    Log.Information($"Updating database to version {version_to_be_updated}");

                    if (_UpdateVersion(version_to_be_updated)) {
                        Log.Information($"Database updated to version {version_to_be_updated}");
                    }
                    else {
                        Log.Error($"Could not update database to version {version_to_be_updated}");
                        throw new Exception();
                    }

                }

            }
            catch (Exception ex) {
                Log.Error($"Updates of database were aborted...");
                Log.Error(ex.Message);
                return false;
            }

            return true;

        }

        /// Soon
        private static bool _UpdateVersion(long version) {
            return true;
        }


    }


}