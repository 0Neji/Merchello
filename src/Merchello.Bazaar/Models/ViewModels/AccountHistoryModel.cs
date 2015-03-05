﻿namespace Merchello.Bazaar.Models.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;

    using Merchello.Web.Models.ContentEditing;

    using Umbraco.Core.Models;
    using Umbraco.Web;

    /// <summary>
    /// The account history model.
    /// </summary>
    public class AccountHistoryModel : MasterModel
    {
        /// <summary>
        /// The _receipt page.
        /// </summary>
        private IPublishedContent _receiptPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountHistoryModel"/> class.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        public AccountHistoryModel(IPublishedContent content)
            : base(content)
        {
        }

        /// <summary>
        /// Gets or sets the invoices.
        /// </summary>
        public IEnumerable<InvoiceDisplay> Invoices { get; set; }

        /// <summary>
        /// Gets the receipt page.
        /// </summary>
        public IPublishedContent ReceiptPage
        {
            get
            {
                return _receiptPage ?? StorePage.Descendant("BazaarReceipt");
            }
        }
    }
}