﻿namespace Merchello.Core.Gateways.Taxation
{
    using System;
    using System.Linq;

    using Merchello.Core.Models;
    using Merchello.Core.Services;

    using Umbraco.Core.Logging;

    /// <summary>
    /// Represents the TaxationContext
    /// </summary>
    internal class TaxationContext : GatewayProviderTypedContextBase<TaxationGatewayProviderBase>, ITaxationContext
    {
        /// <summary>
        /// The _tax by product method.
        /// </summary>
        private ITaxMethod _taxByProductMethod; 

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxationContext"/> class.
        /// </summary>
        /// <param name="gatewayProviderService">
        /// The gateway provider service.
        /// </param>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        public TaxationContext(IGatewayProviderService gatewayProviderService, IGatewayProviderResolver resolver)
            : base(gatewayProviderService, resolver)
        {
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether product pricing enabled.
        /// </summary>
        public bool ProductPricingEnabled { get; internal set; }

        /// <summary>
        /// Gets the product pricing tax method.
        /// </summary>
        internal ITaxMethod ProductPricingTaxMethod
        {
            get
            {
                if (ProductPricingEnabled)
                {
                    return _taxByProductMethod ?? GatewayProviderService.GetTaxMethodForProductPricing();
                }

                return null;
            }
        }

        /// <summary>
        /// Returns an instance of an 'active' GatewayProvider associated with a GatewayMethod based given the unique Key (GUID) of the GatewayMethod
        /// </summary>
        /// <param name="gatewayMethodKey">The unique key (GUID) of the <see cref="IGatewayMethod"/></param>
        /// <returns>An instantiated GatewayProvider</returns>
        public override TaxationGatewayProviderBase GetProviderByMethodKey(Guid gatewayMethodKey)
        {
            return
                GetAllActivatedProviders()
                    .FirstOrDefault(x => ((TaxationGatewayProviderBase)x)
                        .TaxMethods.Any(y => y.Key == gatewayMethodKey)) as TaxationGatewayProviderBase;
        }

        /// <summary>
        /// Calculates taxes for the <see cref="IInvoice"/>
        /// </summary>
        /// <param name="invoice">The <see cref="IInvoice"/> to tax</param>
        /// <param name="quoteOnly">
        /// An optional parameter indicating that the tax calculation should be an estimate.
        /// This is useful for some 3rd party tax APIs
        /// </param>
        /// <returns>The <see cref="ITaxCalculationResult"/></returns>
        /// <remarks>
        /// 
        /// This assumes that the tax rate is associated with the invoice's billing address
        /// 
        /// </remarks>
        public ITaxCalculationResult CalculateTaxesForInvoice(IInvoice invoice, bool quoteOnly = false)
        {
            return CalculateTaxesForInvoice(invoice, invoice.GetBillingAddress());
        }

        /// <summary>
        /// Calculates taxes for the <see cref="IInvoice"/>
        /// </summary>
        /// <param name="invoice">
        /// The <see cref="IInvoice"/> to tax
        /// </param>
        /// <param name="taxAddress">
        /// The address to base the taxation calculation
        /// </param>
        /// <param name="quoteOnly">
        /// An optional parameter indicating that the tax calculation should be an estimate.
        /// This is useful for some 3rd party tax APIs
        /// </param>
        /// <returns>
        /// The <see cref="ITaxCalculationResult"/>
        /// </returns>
        public ITaxCalculationResult CalculateTaxesForInvoice(IInvoice invoice, IAddress taxAddress, bool quoteOnly = false)
        {
            var providersKey =
                GatewayProviderService.GetTaxMethodsByCountryCode(taxAddress.CountryCode)
                                      .Select(x => x.ProviderKey).FirstOrDefault();

            if (Guid.Empty.Equals(providersKey)) return new TaxCalculationResult(0, 0);

            var provider = GatewayProviderResolver.GetProviderByKey<TaxationGatewayProviderBase>(providersKey);

            var gatewayTaxMethod = provider.GetGatewayTaxMethodByCountryCode(taxAddress.CountryCode);

            return gatewayTaxMethod.CalculateTaxForInvoice(invoice, taxAddress);
        }

        /// <summary>
        /// Calculates taxes based on a product.
        /// </summary>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <returns>
        /// The <see cref="ITaxCalculationResult"/>.
        /// </returns>
        public ITaxCalculationResult CalculateTaxesForProduct(IModifiableProductVariantData product)
        {
            if (!ProductPricingEnabled) return new TaxCalculationResult(0, 0);
            if (ProductPricingTaxMethod == null) return new TaxCalculationResult(0, 0);

            var provider =
                GatewayProviderResolver.GetProviderByKey<TaxationGatewayProviderBase>(ProductPricingTaxMethod.ProviderKey);

            if (provider == null)
            {
                var error = new NullReferenceException("Could not reTaxationGatewayProvider for CalculateTaxForProduct could not be resolved");
                LogHelper.Error<TaxationContext>("Resolution failure", error);
                throw error;
            }

            var productProvider = provider as ITaxationByProductProvider;
            if (productProvider != null)
            {
                var method = productProvider.GetTaxationByProductMethod(ProductPricingTaxMethod.Key);
                return method.CalculateTaxForProduct(product);
            }

            LogHelper.Debug<TaxationContext>("Resolved provider did not Implement ITaxationByProductProvider returning no tax");

            return new TaxCalculationResult(0, 0);
        }

        /// <summary>
        /// Gets the tax method for a given tax address
        /// </summary>
        /// <param name="taxAddress">
        /// The tax address
        /// </param>
        /// <returns>
        /// The <see cref="ITaxMethod"/>.
        /// </returns>
        public ITaxMethod GetTaxMethodForTaxAddress(IAddress taxAddress)
        {
            return GetTaxMethodForCountryCode(taxAddress.CountryCode);
        }

        /// <summary>
        /// Gets the tax method for country code.
        /// </summary>
        /// <param name="countryCode">
        /// The country code.
        /// </param>
        /// <returns>
        /// The <see cref="ITaxMethod"/>.
        /// </returns>
        public ITaxMethod GetTaxMethodForCountryCode(string countryCode)
        {
            return GatewayProviderService.GetTaxMethodsByCountryCode(countryCode).FirstOrDefault();
        }


        /// <summary>
        /// Resets the product pricing method to null so that it can be required.
        /// </summary>
        internal void ClearProductPricingMethod()
        {
            _taxByProductMethod = null;
        }
    }
}