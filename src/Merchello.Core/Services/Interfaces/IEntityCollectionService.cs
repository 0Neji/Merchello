﻿namespace Merchello.Core.Services
{
    using System;
    using System.Collections.Generic;

    using Merchello.Core.Models.Interfaces;

    using Umbraco.Core.Services;

    /// <summary>
    /// The EntityCollectionService interface.
    /// </summary>
    public interface IEntityCollectionService : IService
    {
        /// <summary>
        /// The create entity collection.
        /// </summary>
        /// <param name="entityTfKey">
        /// The entity type field key.
        /// </param>
        /// <param name="providerKey">
        /// The provider key.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="raiseEvents">
        /// Optional boolean indicating whether or not to raise events
        /// </param>
        /// <returns>
        /// The <see cref="IEntityCollection"/>.
        /// </returns>
        IEntityCollection CreateEntityCollection(Guid entityTfKey, Guid providerKey, string name, bool raiseEvents = true);

        /// <summary>
        /// The create entity collection with key.
        /// </summary>
        /// <param name="entityTfKey">
        /// The entity type field key.
        /// </param>
        /// <param name="providerKey">
        /// The provider key.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="raiseEvents">
        /// Optional boolean indicating whether or not to raise events
        /// </param>
        /// <returns>
        /// The <see cref="IEntityCollection"/>.
        /// </returns>
        IEntityCollection CreateEntityCollectionWithKey(Guid entityTfKey, Guid providerKey, string name, bool raiseEvents = true);

        /// <summary>
        /// Saves a single entity collection.
        /// </summary>
        /// <param name="entityCollection">
        /// The entity collection.
        /// </param>
        /// <param name="raiseEvents">
        /// Optional boolean indicating whether or not to raise events
        /// </param>
        void Save(IEntityCollection entityCollection, bool raiseEvents = true);

        /// <summary>
        /// Saves a collection of entity collections.
        /// </summary>
        /// <param name="entityCollections">
        /// The entity collections.
        /// </param>
        /// <param name="raiseEvents">
        /// Optional boolean indicating whether or not to raise events
        /// </param>
        void Save(IEnumerable<IEntityCollection> entityCollections, bool raiseEvents = true);

        /// <summary>
        /// Deletes a single entity collection.
        /// </summary>
        /// <param name="entityCollection">
        /// The entity collection.
        /// </param>
        /// <param name="raiseEvents">
        /// Optional boolean indicating whether or not to raise events.
        /// </param>
        void Delete(IEntityCollection entityCollection, bool raiseEvents = true);

        /// <summary>
        /// Deletes a collection of entity collections.
        /// </summary>
        /// <param name="entityCollections">
        /// The entity collections.
        /// </param>
        /// <param name="raiseEvents">
        /// Optional boolean indicating whether or not to raise events.
        /// </param>
        void Delete(IEnumerable<IEntityCollection> entityCollections, bool raiseEvents = true);

        /// <summary>
        /// The get by key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="IEntityCollection"/>.
        /// </returns>
        IEntityCollection GetByKey(Guid key);

        /// <summary>
        /// The get by entity type field key.
        /// </summary>
        /// <param name="entityTfKey">
        /// The entity type field key.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IEntityCollection}"/>.
        /// </returns>
        IEnumerable<IEntityCollection> GetByEntityTfKey(Guid entityTfKey);

        /// <summary>
        /// The get by provider key.
        /// </summary>
        /// <param name="providerKey">
        /// The provider key.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IEntityCollection}"/>.
        /// </returns>
        IEnumerable<IEntityCollection> GetByProviderKey(Guid providerKey);
    }
}