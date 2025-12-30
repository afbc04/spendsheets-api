
public class ConfigDAO {

    private Config? _config;

    public ConfigDAO() {
        this._config = null;
    }

    public Config? get() {
        return this._config;
    }

    public void put(Config config) {
        this._config = config;
    }

}