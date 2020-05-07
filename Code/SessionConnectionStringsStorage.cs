using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class SessionConnectionStringsStorage : IDataSourceWizardConnectionStringsStorage {
    const string ConnectionsStorageKey = "f385f088-d238-45af-84eb-be0eb490c22d";
    const string DefaultConnectionStringsSectionName = "ConnectionStrings";

    readonly IHttpContextAccessor contextAccessor;
    readonly IConfiguration configuration;
    HttpContext HttpContext { get { return contextAccessor?.HttpContext; } }

    public SessionConnectionStringsStorage(IConfiguration configuration, IHttpContextAccessor contextAccessor) : base() {
        this.configuration = configuration;
        this.contextAccessor = contextAccessor;
    }

    Dictionary<string, DataConnectionParametersBase> GetOrCreateStorage() {
        var storage = GetFromSession();
        if(storage == null) {
            storage = CreateStorage();
            SaveToSession(storage);
        }
        return storage;
    }
    Dictionary<string, DataConnectionParametersBase> CreateStorage() {
        if(configuration == null)
            return new Dictionary<string, DataConnectionParametersBase>();
        return configuration.GetSection(DefaultConnectionStringsSectionName).GetChildren()
            .ToDictionary(connStr => connStr.Key, connStr => (DataConnectionParametersBase)new CustomStringConnectionParameters(connStr.Value));
    }
    Dictionary<string, DataConnectionParametersBase> GetFromSession() {
        ISession session = HttpContext?.Session;
        if(session == null)
            return null;
        string serializedStorage = session.GetString(ConnectionsStorageKey);
        if(serializedStorage == null)
            return null;
        var connStrStorage = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedStorage);
        return connStrStorage.ToDictionary(p => p.Key, p => (DataConnectionParametersBase)new CustomStringConnectionParameters(p.Value));
    }
    void SaveToSession(Dictionary<string, DataConnectionParametersBase> storage) {
        HttpContext?.Session?.SetString(
            ConnectionsStorageKey,
            JsonConvert.SerializeObject(storage.ToDictionary(p => p.Key, p => GetConnectionString(p.Value)))
        );
    }
    static string GetConnectionString(DataConnectionParametersBase connectionPars) {
        CustomStringConnectionParameters customConnectionPars = connectionPars as CustomStringConnectionParameters;
        if(customConnectionPars != null)
            return customConnectionPars.ConnectionString;
        JsonSourceConnectionParameters jsonConnectionPars = connectionPars as JsonSourceConnectionParameters;
        if(jsonConnectionPars != null) {
            if(jsonConnectionPars.JsonSource == null)
                throw new Exception();
            JsonDataConnection jsonConnection = new JsonDataConnection(jsonConnectionPars.JsonSource);
            return jsonConnection.CreateConnectionString();
        }
        throw new Exception();
    }

    Dictionary<string, string> IDataSourceWizardConnectionStringsProvider.GetConnectionDescriptions() {
        return GetOrCreateStorage().Keys.ToDictionary(key => key, key => key);
    }
    DataConnectionParametersBase IDataSourceWizardConnectionStringsProvider.GetDataConnectionParameters(string name) {
        DataConnectionParametersBase result;
        GetOrCreateStorage().TryGetValue(name, out result);
        return result;
    }
    void IDataSourceWizardConnectionStringsStorage.SaveDataConnectionParameters(string name, DataConnectionParametersBase connectionParameters, bool saveCredentials) {
        var storage = GetOrCreateStorage();
        storage[name] = connectionParameters;
        SaveToSession(storage);
    }
}
