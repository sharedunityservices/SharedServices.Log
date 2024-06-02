# SharedServices.Log
This package provides a simple logging service for use in other packages.

### Note
If you would like to set settings other than default using `config.json`, you should implement `IOverrideServices` in 
your project and add `services[typeof(ILogService)] = new Log()` to the `OverrideServices` method.