﻿namespace Merchello.Core.Persistence.Migrations
{
    using System;
    using System.Linq;

    using Merchello.Core.Configuration;
    using Merchello.Core.Events;
    using Merchello.Core.Persistence.Migrations.Analytics;
    using Merchello.Core.Persistence.Migrations.Initial;

    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.Migrations;

    using Constants = Merchello.Core.Constants;

    /// <summary>
    /// The merchello upgrade helper.
    /// </summary>
    internal class CoreMigrationManager
    {
        /// <summary>
        /// The <see cref="Database"/>.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreMigrationManager"/> class.
        /// </summary>
        public CoreMigrationManager()
            : this(ApplicationContext.Current.DatabaseContext.Database)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreMigrationManager"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        public CoreMigrationManager(Database database)
        {
            Mandate.ParameterNotNull(database, "database");
            _database = database;
        }

        /// <summary>
        /// The delegate for the upgraded event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="MerchelloMigrationEventArgs"/>.
        /// </param>
        public delegate void UpgradedEventHandler(object sender, MerchelloMigrationEventArgs e);

        /// <summary>
        /// The upgraded event.
        /// </summary>
        public event UpgradedEventHandler Upgraded;

        /// <summary>
        /// Checks the binary version against the web.config configuration status version.
        /// </summary>
        public void EnsureMerchelloVersion()
        {
            if (MerchelloConfiguration.ConfigurationStatusVersion != MerchelloVersion.Current)
            {
                LogHelper.Info<CoreMigrationManager>(
                    "Merchello Versions did not match - initializing upgrade.");

                if (UpgradeMerchello(_database))
                {
                    LogHelper.Info<CoreMigrationManager>("Upgrade completed successfully.");
                }
            }
            else
            {
                LogHelper.Debug<CoreMigrationManager>("Merchello Version Verified - no upgrade required.");
            }
        }

        /// <summary>
        /// Executes the Migration runner.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <returns>
        /// A value indicating whether or not the migration was successful.
        /// </returns>
        private bool UpgradeMerchello(Database database)
        {
            var databaseSchemaCreation = new DatabaseSchemaCreation(database);
            var schemaResult = databaseSchemaCreation.ValidateSchema();
            var dbVersion = schemaResult.DetermineInstalledVersion();

            if (dbVersion != MerchelloVersion.Current)
            {
                try
                {
                    LogHelper.Info<CoreMigrationManager>("Merchello database upgraded required.  Initializing Upgrade.");
                    var runner = new MigrationRunner(MerchelloConfiguration.ConfigurationStatusVersion, MerchelloVersion.Current, MerchelloConfiguration.MerchelloMigrationName);
                    var upgraded = runner.Execute(database);
                    if (upgraded)
                    {
                        var migrationSetting = schemaResult.StoreSettings.FirstOrDefault(x => x.Key == Constants.StoreSettingKeys.MigrationKey);
                        var migrationKey = migrationSetting != null ? migrationSetting.Value : Guid.NewGuid().ToString();
                        
                        var record = new MigrationRecord()
                                         {
                                             MigrationKey = migrationKey,
                                             CurrentVersion = dbVersion.ToString(),
                                             TargetVersion = MerchelloVersion.Current.ToString(),
                                             DbProvider = database.GetDatabaseProvider().ToString(),
                                             InstallDate = DateTime.Now,
                                             IsUpgrade = true
                                         };

                        this.OnUpgraded(record);

                        LogHelper.Info<CoreMigrationManager>("Merchello Schema Migration completed successfully");
                    }

                    LogHelper.Debug<CoreMigrationManager>("Merchello migration runner returned false.");
                }
                catch (Exception ex)
                {
                    LogHelper.Error<CoreMigrationManager>("Merchello Database Schema Upgrade Failed", ex);
                    throw;
                }
            }
            
            MerchelloConfiguration.ConfigurationStatus = MerchelloVersion.Current.ToString();

            return true;
        }

        /// <summary>
        /// The on upgraded.
        /// </summary>
        /// <param name="record">
        /// The record.
        /// </param>
        private void OnUpgraded(MigrationRecord record)
        {
            if (Upgraded != null)
            {
                Upgraded(this, new MerchelloMigrationEventArgs(record));
            }
        }
    }
}